using Anzuelo.Application.Config;
using Anzuelo.Application.DTOs;
using Anzuelo.Infraestructure.Models;
using Anzuelo.Application.Services.Interfaces;
using Anzuelo.Infraestructure.Repository.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;
namespace Anzuelo.Application.Services.Implementations
{
    public class ServiceFacturaPedido : IServiceFacturaPedido
    {
        private const string ROL_CLIENTE = "Cliente";

        public static void AsignarNombresUsuarios(Pedido entidad, PedidoDTO dto)
        {
            var usuarioCliente = entidad.IdUsuario
                .FirstOrDefault(u => u.IdRolNavigation.NombreRol.Equals(ROL_CLIENTE, StringComparison.OrdinalIgnoreCase));

            var usuarioEncargado = entidad.IdUsuario
                .FirstOrDefault(u => !u.IdRolNavigation.NombreRol.Equals(ROL_CLIENTE, StringComparison.OrdinalIgnoreCase));

            dto.NombreCliente = usuarioCliente != null ? $"{usuarioCliente.Nombre} {usuarioCliente.Apellido1}" : string.Empty;
            dto.CedulaCliente = usuarioCliente?.Cedula ?? string.Empty;
            dto.NombreEncargado = usuarioEncargado != null ? $"{usuarioEncargado.Nombre} {usuarioEncargado.Apellido1}" : string.Empty;
        }

        private readonly IRepositoryPedido _repositoryPedido;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;
        private readonly ILogger<ServiceFacturaPedido> _logger;

        public ServiceFacturaPedido(
            IRepositoryPedido repositoryPedido,
            IMapper mapper,
            IOptions<AppConfig> options,
            ILogger<ServiceFacturaPedido> logger)
        {
            _repositoryPedido = repositoryPedido;
            _mapper = mapper;
            _options = options;
            _logger = logger;
        }

        public async Task<byte[]> GenerarFacturaAsync(int idPedido)
        {
            var entidad = await _repositoryPedido.FindByIdAsync(idPedido)
                ?? throw new InvalidOperationException($"El pedido con id {idPedido} no existe.");

            var dto = _mapper.Map<PedidoDTO>(entidad);
            AsignarNombresUsuarios(entidad, dto);

            QuestPDF.Settings.License = LicenseType.Community;

            var pdfByteArray = QuestPDF.Fluent.Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(30);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Soda Anzuelo").Bold().FontSize(16).FontColor("#0a2f5a");
                                c.Item().Text("Puntarenas, Costa Rica").FontSize(9);
                                c.Item().Text("Tel: 2222-2222 | contacto@anzuelo.com").FontSize(9);
                            });
                            row.ConstantItem(150).Column(c =>
                            {
                                c.Item().AlignRight().Text($"Pedido #{dto.IdPedido}").Bold().FontSize(12);
                                c.Item().AlignRight().Text(dto.FechaPedido.ToString("dd/MM/yyyy HH:mm")).FontSize(9);
                                c.Item().AlignRight().Text(dto.NombreEstado).FontSize(9).FontColor("#e67e22");
                            });
                        });
                        col.Item().PaddingTop(8).LineHorizontal(1).LineColor("#0a2f5a");
                    });

                    page.Content().PaddingVertical(15).Column(col =>
                    {
                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Cliente").Bold().FontSize(10).FontColor("#0a2f5a");
                                c.Item().Text(dto.NombreCliente ?? string.Empty);
                                c.Item().Text($"Cédula: {dto.CedulaCliente}").FontSize(9);
                            });

                            if (!string.IsNullOrWhiteSpace(dto.NombreEncargado))
                            {
                                row.RelativeItem().Column(c =>
                                {
                                    c.Item().Text("Encargado").Bold().FontSize(10).FontColor("#0a2f5a");
                                    c.Item().Text(dto.NombreEncargado);
                                });
                            }

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Entrega").Bold().FontSize(10).FontColor("#0a2f5a");
                                c.Item().Text(dto.NombreTipoEntrega ?? string.Empty);
                            });

                            row.RelativeItem().Column(c =>
                            {
                                c.Item().Text("Pago").Bold().FontSize(10).FontColor("#0a2f5a");
                                c.Item().Text(dto.Pago?.NombreMetodoPago ?? string.Empty);
                            });
                        });

                        col.Item().PaddingTop(15).Table(tabla =>
                        {
                            tabla.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(1.2f);
                                columns.RelativeColumn(2);
                            });

                            tabla.Header(header =>
                            {
                                header.Cell().Background("#0a2f5a").Padding(5).Text("Producto/Combo").FontColor("#fff").FontSize(9);
                                header.Cell().Background("#0a2f5a").Padding(5).AlignRight().Text("Precio").FontColor("#fff").FontSize(9);
                                header.Cell().Background("#0a2f5a").Padding(5).AlignCenter().Text("Cant.").FontColor("#fff").FontSize(9);
                                header.Cell().Background("#0a2f5a").Padding(5).AlignRight().Text("Subtotal").FontColor("#fff").FontSize(9);
                                header.Cell().Background("#0a2f5a").Padding(5).AlignRight().Text("Impuesto").FontColor("#fff").FontSize(9);
                                header.Cell().Background("#0a2f5a").Padding(5).Text("Obs.").FontColor("#fff").FontSize(9);
                            });

                            foreach (var linea in dto.Detalles)
                            {
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(5).Text(linea.Nombre ?? string.Empty).FontSize(9);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(5).AlignRight().Text("₡" + linea.PrecioUnitario.ToString("N2")).FontSize(9);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(5).AlignCenter().Text(linea.Cantidad.ToString()).FontSize(9);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(5).AlignRight().Text("₡" + linea.Subtotal.ToString("N2")).FontSize(9);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(5).AlignRight().Text("₡" + linea.Impuesto.ToString("N2")).FontSize(9);
                                tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9").Padding(5).Text(linea.Observaciones ?? string.Empty).FontSize(8);
                            }
                        });

                        col.Item().PaddingTop(15).AlignRight().Width(220).Column(c =>
                        {
                            c.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Subtotal").FontSize(9);
                                r.ConstantItem(90).AlignRight().Text("₡" + dto.Subtotal.ToString("N2")).FontSize(9);
                            });
                            c.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Impuesto").FontSize(9);
                                r.ConstantItem(90).AlignRight().Text("₡" + dto.Impuesto.ToString("N2")).FontSize(9);
                            });
                            if (dto.CostoEnvio > 0)
                            {
                                c.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Costo de envío").FontSize(9);
                                    r.ConstantItem(90).AlignRight().Text("₡" + dto.CostoEnvio.ToString("N2")).FontSize(9);
                                });
                            }
                            c.Item().PaddingTop(4).LineHorizontal(1).LineColor("#e67e22");
                            c.Item().Row(r =>
                            {
                                r.RelativeItem().Text("Total").Bold().FontSize(12);
                                r.ConstantItem(90).AlignRight().Text("₡" + dto.Total.ToString("N2")).Bold().FontSize(12).FontColor("#e67e22");
                            });

                            if (dto.Pago != null && dto.Pago.NombreMetodoPago != null &&
                                dto.Pago.NombreMetodoPago.Contains("efectivo", StringComparison.OrdinalIgnoreCase))
                            {
                                c.Item().PaddingTop(6).Row(r =>
                                {
                                    r.RelativeItem().Text("Monto recibido").FontSize(9);
                                    r.ConstantItem(90).AlignRight().Text("₡" + (dto.Pago.MontoRecibido ?? 0).ToString("N2")).FontSize(9);
                                });
                                c.Item().Row(r =>
                                {
                                    r.RelativeItem().Text("Vuelto").FontSize(9);
                                    r.ConstantItem(90).AlignRight().Text("₡" + (dto.Pago.Vuelto ?? 0).ToString("N2")).FontSize(9);
                                });
                            }
                            else if (dto.Pago != null && !string.IsNullOrWhiteSpace(dto.Pago.Ultimos4Tarjeta))
                            {
                                c.Item().PaddingTop(6).Row(r =>
                                {
                                    r.RelativeItem().Text("Tarjeta terminada en").FontSize(9);
                                    r.ConstantItem(90).AlignRight().Text("**** " + dto.Pago.Ultimos4Tarjeta).FontSize(9);
                                });
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(txt =>
                    {
                        txt.Span("Gracias por su compra - Soda Anzuelo").FontSize(8);
                    });
                });
            }).GeneratePdf();

            return pdfByteArray;
        }

        public async Task<bool> EnviarFacturaAsync(int idPedido, string email)
        {
            if (string.IsNullOrEmpty(_options.Value.SmtpConfiguration.Server))
            {
                _logger.LogError($"No se encuentra configurado SMTP en {MethodBase.GetCurrentMethod()!.DeclaringType!.FullName}");
                return false;
            }

            var pdfBytes = await GenerarFacturaAsync(idPedido);

            var mailMessage = new MailMessage(
                    new MailAddress(_options.Value.SmtpConfiguration.UserName, _options.Value.SmtpConfiguration.FromName),
                    new MailAddress(email))
            {
                Subject = $"Factura de su pedido #{idPedido} - Soda Anzuelo",
                Body = "Adjunto encontrará la factura correspondiente a su pedido en Soda Anzuelo. ¡Gracias por su compra!",
                IsBodyHtml = true
            };

            using var stream = new MemoryStream(pdfBytes);
            mailMessage.Attachments.Add(new Attachment(stream, $"Factura_Pedido_{idPedido}.pdf", "application/pdf"));

            using var smtpClient = new SmtpClient(_options.Value.SmtpConfiguration.Server, _options.Value.SmtpConfiguration.PortNumber)
            {
                Credentials = new NetworkCredential(_options.Value.SmtpConfiguration.UserName, _options.Value.SmtpConfiguration.Password),
                EnableSsl = _options.Value.SmtpConfiguration.EnableSsl,
                UseDefaultCredentials = false
            };

            await smtpClient.SendMailAsync(mailMessage);

            return true;
        }
    }
}

  

