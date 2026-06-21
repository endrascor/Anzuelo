using System;
using System.Collections.Generic;
using Anzuelo.Infraestructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Anzuelo.Infraestructure.Data;

public partial class AnzueloContext : DbContext
{
    public AnzueloContext(DbContextOptions<AnzueloContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaCombo> CategoriaCombo { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProducto { get; set; }

    public virtual DbSet<Combo> Combo { get; set; }

    public virtual DbSet<ComboProducto> ComboProducto { get; set; }

    public virtual DbSet<DetallePedido> DetallePedido { get; set; }

    public virtual DbSet<Direccion> Direccion { get; set; }

    public virtual DbSet<Disponibilidad> Disponibilidad { get; set; }

    public virtual DbSet<DisponibilidadDia> DisponibilidadDia { get; set; }

    public virtual DbSet<EstacionCocina> EstacionCocina { get; set; }

    public virtual DbSet<EstadoCombo> EstadoCombo { get; set; }

    public virtual DbSet<EstadoMenu> EstadoMenu { get; set; }

    public virtual DbSet<EstadoPedido> EstadoPedido { get; set; }

    public virtual DbSet<EstadoPedidoEstacion> EstadoPedidoEstacion { get; set; }

    public virtual DbSet<EstadoProducto> EstadoProducto { get; set; }

    public virtual DbSet<EstadoUsuario> EstadoUsuario { get; set; }

    public virtual DbSet<ImagenProducto> ImagenProducto { get; set; }

    public virtual DbSet<Ingrediente> Ingrediente { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<MenuCombo> MenuCombo { get; set; }

    public virtual DbSet<MenuProducto> MenuProducto { get; set; }

    public virtual DbSet<MetodoPago> MetodoPago { get; set; }

    public virtual DbSet<Pago> Pago { get; set; }

    public virtual DbSet<Pedido> Pedido { get; set; }

    public virtual DbSet<PedidoEstacion> PedidoEstacion { get; set; }

    public virtual DbSet<Preparacion> Preparacion { get; set; }

    public virtual DbSet<PreparacionEstacion> PreparacionEstacion { get; set; }

    public virtual DbSet<Producto> Producto { get; set; }

    public virtual DbSet<ProductoIngrediente> ProductoIngrediente { get; set; }

    public virtual DbSet<Rol> Rol { get; set; }

    public virtual DbSet<SeguimientoPedido> SeguimientoPedido { get; set; }

    public virtual DbSet<TipoEntrega> TipoEntrega { get; set; }

    public virtual DbSet<Usuario> Usuario { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoriaCombo>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaCombo).HasName("PK__Categori__5260DA131FAE363F");

            entity.HasIndex(e => e.Descripcion, "UQ__Categori__298336B638F51A2A").IsUnique();

            entity.Property(e => e.IdCategoriaCombo).HasColumnName("idCategoriaCombo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto).HasName("PK__Categori__88F047C54230ACC0");

            entity.HasIndex(e => e.Descripcion, "UQ__Categori__298336B624C5E85A").IsUnique();

            entity.Property(e => e.IdCategoriaProducto).HasColumnName("idCategoriaProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Combo>(entity =>
        {
            entity.HasKey(e => e.IdCombo).HasName("PK__Combo__4F02C7C120EE9472");

            entity.HasIndex(e => e.Nombre, "UQ__Combo__72AFBCC6833854F4").IsUnique();

            entity.Property(e => e.IdCombo).HasColumnName("idCombo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoriaCombo).HasColumnName("idCategoriaCombo");
            entity.Property(e => e.IdEstadoCombo).HasColumnName("idEstadoCombo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PrecioTotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioTotal");

            entity.HasOne(d => d.IdCategoriaComboNavigation).WithMany(p => p.Combo)
                .HasForeignKey(d => d.IdCategoriaCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Combo_Categoria");

            entity.HasOne(d => d.IdEstadoComboNavigation).WithMany(p => p.Combo)
                .HasForeignKey(d => d.IdEstadoCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Combo_Estado");
        });

        modelBuilder.Entity<ComboProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdCombo, e.IdProducto }).HasName("PK__ComboPro__7F7D8DD28F62042A");

            entity.Property(e => e.IdCombo).HasColumnName("idCombo");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Cantidad)
                .HasDefaultValue(1)
                .HasColumnName("cantidad");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.ComboProducto)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboProducto_Combo");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ComboProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ComboProducto_Producto");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__DetalleP__610F00567A2760C7");

            entity.Property(e => e.IdDetallePedido).HasColumnName("idDetallePedido");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.IdComboProducto).HasColumnName("idComboProducto");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Observaciones)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("observaciones");
            entity.Property(e => e.PrecioUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precioUnitario");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetallePedido_Pedido");
        });

        modelBuilder.Entity<Direccion>(entity =>
        {
            entity.HasKey(e => e.IdDireccion);

            entity.Property(e => e.IdDireccion).HasColumnName("idDireccion");
            entity.Property(e => e.Canton)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("canton");
            entity.Property(e => e.Detalle)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("detalle");
            entity.Property(e => e.Distrito)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("distrito");
            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Provincia)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("provincia");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Direccion)
                .HasForeignKey(d => d.IdUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Direccion_Usuario");
        });

        modelBuilder.Entity<Disponibilidad>(entity =>
        {
            entity.HasKey(e => e.IdDisponibilidad).HasName("PK__Disponib__96A3EB6AB2274FCA");

            entity.Property(e => e.IdDisponibilidad).HasColumnName("idDisponibilidad");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.FechaFinal)
                .HasColumnType("datetime")
                .HasColumnName("fechaFinal");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.HoraFinal)
                .HasColumnType("datetime")
                .HasColumnName("horaFinal");
            entity.Property(e => e.HoraInicio)
                .HasColumnType("datetime")
                .HasColumnName("horaInicio");
            entity.Property(e => e.IdDisponibilidadDia).HasColumnName("idDisponibilidadDia");

            entity.HasOne(d => d.IdDisponibilidadDiaNavigation).WithMany(p => p.Disponibilidad)
                .HasForeignKey(d => d.IdDisponibilidadDia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Disponibilidad_DisponibilidadDia");
        });

        modelBuilder.Entity<DisponibilidadDia>(entity =>
        {
            entity.HasKey(e => e.IdDisponibilidadDia);

            entity.Property(e => e.IdDisponibilidadDia).HasColumnName("idDisponibilidadDia");
            entity.Property(e => e.DiaSemana)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("diaSemana");
        });

        modelBuilder.Entity<EstacionCocina>(entity =>
        {
            entity.HasKey(e => e.IdEstacionCocina).HasName("PK__Estacion__C36AEFF00B7520F3");

            entity.HasIndex(e => e.Descripcion, "UQ__Estacion__298336B60E3CCD9F").IsUnique();

            entity.Property(e => e.IdEstacionCocina).HasColumnName("idEstacionCocina");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<EstadoCombo>(entity =>
        {
            entity.HasKey(e => e.IdEstadoCombo).HasName("PK__EstadoCo__FF6DC1D93178CEF4");

            entity.HasIndex(e => e.Descripcion, "UQ__EstadoCo__298336B685E01F7C").IsUnique();

            entity.Property(e => e.IdEstadoCombo).HasColumnName("idEstadoCombo");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<EstadoMenu>(entity =>
        {
            entity.HasKey(e => e.IdEstadoMenu).HasName("PK__EstadoMe__DE586D1918256AEB");

            entity.HasIndex(e => e.Descripcion, "UQ__EstadoMe__298336B606B32CBD").IsUnique();

            entity.Property(e => e.IdEstadoMenu).HasColumnName("idEstadoMenu");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<EstadoPedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedido).HasName("PK__EstadoPe__E0BFAD0F00138644");

            entity.HasIndex(e => e.Descripcion, "UQ__EstadoPe__298336B604D44C45").IsUnique();

            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<EstadoPedidoEstacion>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedidoEstacion).HasName("PK__EstadoPe__F34EFCCD076A2375");

            entity.HasIndex(e => e.Descripcion, "UQ__EstadoPe__298336B656AC1A43").IsUnique();

            entity.Property(e => e.IdEstadoPedidoEstacion).HasColumnName("idEstadoPedidoEstacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<EstadoProducto>(entity =>
        {
            entity.HasKey(e => e.IdEstadoProducto).HasName("PK__EstadoPr__C7C0DA9DED012BC3");

            entity.HasIndex(e => e.Descripcion, "UQ__EstadoPr__298336B64964FA1C").IsUnique();

            entity.Property(e => e.IdEstadoProducto).HasColumnName("idEstadoProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<EstadoUsuario>(entity =>
        {
            entity.HasKey(e => e.IdEstadoUsuario).HasName("PK__EstadoUs__570885738B5FE7AF");

            entity.HasIndex(e => e.NombreEstado, "UQ__EstadoUs__F272D1EE147002DE").IsUnique();

            entity.Property(e => e.IdEstadoUsuario).HasColumnName("idEstadoUsuario");
            entity.Property(e => e.NombreEstado)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombreEstado");
        });

        modelBuilder.Entity<ImagenProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagenProducto).HasName("PK__ImagenPr__7027B7708E445C49");

            entity.Property(e => e.IdImagenProducto).HasColumnName("idImagenProducto");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Imagen)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("imagen");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ImagenProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ImagenProducto_Producto");
        });

        modelBuilder.Entity<Ingrediente>(entity =>
        {
            entity.HasKey(e => e.IdIngrediente).HasName("PK__Ingredie__563C0D3338D6279D");

            entity.HasIndex(e => e.NombreIngrediente, "UQ__Ingredie__92BF05289511B5AE").IsUnique();

            entity.Property(e => e.IdIngrediente).HasColumnName("idIngrediente");
            entity.Property(e => e.NombreIngrediente)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombreIngrediente");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.IdMenu).HasName("PK__Menu__C26AF483AD9F7982");

            entity.HasIndex(e => e.NombreMenu, "UQ__Menu__E0E7063B3E215FCF").IsUnique();

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdDisponibilidad).HasColumnName("idDisponibilidad");
            entity.Property(e => e.IdEstadoMenu).HasColumnName("idEstadoMenu");
            entity.Property(e => e.NombreMenu)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombreMenu");

            entity.HasOne(d => d.IdDisponibilidadNavigation).WithMany(p => p.Menu)
                .HasForeignKey(d => d.IdDisponibilidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_Disponibilidad");

            entity.HasOne(d => d.IdEstadoMenuNavigation).WithMany(p => p.Menu)
                .HasForeignKey(d => d.IdEstadoMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_Estado");
        });

        modelBuilder.Entity<MenuCombo>(entity =>
        {
            entity.HasKey(e => new { e.IdMenu, e.IdCombo }).HasName("PK__MenuComb__C69AD8FF2C0141D2");

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdCombo).HasColumnName("idCombo");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("descuento");

            entity.HasOne(d => d.IdComboNavigation).WithMany(p => p.MenuCombo)
                .HasForeignKey(d => d.IdCombo)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuCombo_Combo");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuCombo)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuCombo_Menu");
        });

        modelBuilder.Entity<MenuProducto>(entity =>
        {
            entity.HasKey(e => new { e.IdMenu, e.IdProducto }).HasName("PK__MenuProd__F215BE90172E74A1");

            entity.Property(e => e.IdMenu).HasColumnName("idMenu");
            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descuento)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("descuento");

            entity.HasOne(d => d.IdMenuNavigation).WithMany(p => p.MenuProducto)
                .HasForeignKey(d => d.IdMenu)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuProducto_Menu");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.MenuProducto)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuProducto_Producto");
        });

        modelBuilder.Entity<MetodoPago>(entity =>
        {
            entity.HasKey(e => e.IdMetodoPago).HasName("PK__MetodoPa__817BFC328B1A68EF");

            entity.HasIndex(e => e.Descripcion, "UQ__MetodoPa__298336B6D9644562").IsUnique();

            entity.Property(e => e.IdMetodoPago).HasColumnName("idMetodoPago");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.IdPago).HasName("PK__Pago__BD2295ADD6343683");

            entity.Property(e => e.IdPago).HasColumnName("idPago");
            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaPago");
            entity.Property(e => e.IdMetodoPago).HasColumnName("idMetodoPago");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Monto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("monto");
            entity.Property(e => e.MontoRecibido)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("montoRecibido");
            entity.Property(e => e.Ultimos4Tarjeta)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("ultimos4Tarjeta");
            entity.Property(e => e.Vuelto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("vuelto");

            entity.HasOne(d => d.IdMetodoPagoNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdMetodoPago)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_MetodoPago");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.Pago)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pago_Pedido");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedido__A9F619B7ED981E94");

            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.CostoEnvio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("costoEnvio");
            entity.Property(e => e.FechaPedido)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaPedido");
            entity.Property(e => e.IdDireccion).HasColumnName("idDireccion");
            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.IdTipoEntrega).HasColumnName("idTipoEntrega");
            entity.Property(e => e.Impuesto)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("impuesto");
            entity.Property(e => e.Subtotal)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("subtotal");
            entity.Property(e => e.Total)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total");

            entity.HasOne(d => d.IdDireccionNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdDireccion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Direccion");

            entity.HasOne(d => d.IdEstadoPedidoNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdEstadoPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_Estado");

            entity.HasOne(d => d.IdTipoEntregaNavigation).WithMany(p => p.Pedido)
                .HasForeignKey(d => d.IdTipoEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pedido_TipoEntrega");

            entity.HasMany(d => d.IdUsuario).WithMany(p => p.IdPedido)
                .UsingEntity<Dictionary<string, object>>(
                    "PedidoUsuario",
                    r => r.HasOne<Usuario>().WithMany()
                        .HasForeignKey("IdUsuario")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PedidoUsuario_Usuario"),
                    l => l.HasOne<Pedido>().WithMany()
                        .HasForeignKey("IdPedido")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_PedidoUsuario_Pedido"),
                    j =>
                    {
                        j.HasKey("IdPedido", "IdUsuario");
                        j.IndexerProperty<int>("IdPedido").HasColumnName("idPedido");
                        j.IndexerProperty<int>("IdUsuario").HasColumnName("idUsuario");
                    });
        });

        modelBuilder.Entity<PedidoEstacion>(entity =>
        {
            entity.HasKey(e => e.IdPedidoEstacion).HasName("PK__PedidoEs__BA98891BD35DFE3D");

            entity.Property(e => e.IdPedidoEstacion).HasColumnName("idPedidoEstacion");
            entity.Property(e => e.FechaFin)
                .HasColumnType("datetime")
                .HasColumnName("fechaFin");
            entity.Property(e => e.FechaInicio)
                .HasColumnType("datetime")
                .HasColumnName("fechaInicio");
            entity.Property(e => e.IdDetallePedido).HasColumnName("idDetallePedido");
            entity.Property(e => e.IdEstacionCocina).HasColumnName("idEstacionCocina");
            entity.Property(e => e.IdEstadoPedidoEstacion).HasColumnName("idEstadoPedidoEstacion");
            entity.Property(e => e.OrdenProceso).HasColumnName("ordenProceso");
            entity.Property(e => e.TiempoEstimadoMinutos).HasColumnName("tiempoEstimadoMinutos");

            entity.HasOne(d => d.IdDetallePedidoNavigation).WithMany(p => p.PedidoEstacion)
                .HasForeignKey(d => d.IdDetallePedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoEstacion_Detalle");

            entity.HasOne(d => d.IdEstacionCocinaNavigation).WithMany(p => p.PedidoEstacion)
                .HasForeignKey(d => d.IdEstacionCocina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoEstacion_Estacion");

            entity.HasOne(d => d.IdEstadoPedidoEstacionNavigation).WithMany(p => p.PedidoEstacion)
                .HasForeignKey(d => d.IdEstadoPedidoEstacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PedidoEstacion_Estado");
        });

        modelBuilder.Entity<Preparacion>(entity =>
        {
            entity.HasKey(e => e.IdPreparacion).HasName("PK__Preparac__7590B0A4C7DCC877");

            entity.Property(e => e.IdPreparacion).HasColumnName("idPreparacion");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<PreparacionEstacion>(entity =>
        {
            entity.HasKey(e => new { e.IdPreparacion, e.IdEstacionCocina }).HasName("PK__Preparac__69A61E5B195EBA84");

            entity.Property(e => e.IdPreparacion).HasColumnName("idPreparacion");
            entity.Property(e => e.IdEstacionCocina).HasColumnName("idEstacionCocina");
            entity.Property(e => e.NumeroOrden).HasColumnName("numeroOrden");
            entity.Property(e => e.TiempoEstimadoMinutos).HasColumnName("tiempoEstimadoMinutos");

            entity.HasOne(d => d.IdEstacionCocinaNavigation).WithMany(p => p.PreparacionEstacion)
                .HasForeignKey(d => d.IdEstacionCocina)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreparacionEstacion_Estacion");

            entity.HasOne(d => d.IdPreparacionNavigation).WithMany(p => p.PreparacionEstacion)
                .HasForeignKey(d => d.IdPreparacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PreparacionEstacion_Preparacion");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__07F4A1325D71493D");

            entity.HasIndex(e => e.Nombre, "UQ__Producto__72AFBCC668EF21A2").IsUnique();

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.IdCategoriaProducto).HasColumnName("idCategoriaProducto");
            entity.Property(e => e.IdEstadoProducto).HasColumnName("idEstadoProducto");
            entity.Property(e => e.IdPreparacion).HasColumnName("idPreparacion");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("precio");

            entity.HasOne(d => d.IdCategoriaProductoNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdCategoriaProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Categoria");

            entity.HasOne(d => d.IdEstadoProductoNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdEstadoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Estado");

            entity.HasOne(d => d.IdPreparacionNavigation).WithMany(p => p.Producto)
                .HasForeignKey(d => d.IdPreparacion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Producto_Preparacion");
        });

        modelBuilder.Entity<ProductoIngrediente>(entity =>
        {
            entity.HasKey(e => new { e.IdProducto, e.IdIngrediente }).HasName("PK__Producto__229761E1A7DBDC93");

            entity.Property(e => e.IdProducto).HasColumnName("idProducto");
            entity.Property(e => e.IdIngrediente).HasColumnName("idIngrediente");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");

            entity.HasOne(d => d.IdIngredienteNavigation).WithMany(p => p.ProductoIngrediente)
                .HasForeignKey(d => d.IdIngrediente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductoIngrediente_Ingrediente");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.ProductoIngrediente)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductoIngrediente_Producto");
        });

        modelBuilder.Entity<Rol>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Rol__3C872F769797A8A1");

            entity.HasIndex(e => e.NombreRol, "UQ__Rol__2787B00C2E516CFD").IsUnique();

            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombreRol");
        });

        modelBuilder.Entity<SeguimientoPedido>(entity =>
        {
            entity.HasKey(e => e.IdSeguimientoPedido).HasName("PK__Seguimie__B9128F90BD5D2BC8");

            entity.Property(e => e.IdSeguimientoPedido).HasColumnName("idSeguimientoPedido");
            entity.Property(e => e.FechaCambio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaCambio");
            entity.Property(e => e.IdEstadoPedido).HasColumnName("idEstadoPedido");
            entity.Property(e => e.IdPedido).HasColumnName("idPedido");
            entity.Property(e => e.Observacion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("observacion");

            entity.HasOne(d => d.IdEstadoPedidoNavigation).WithMany(p => p.SeguimientoPedido)
                .HasForeignKey(d => d.IdEstadoPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoPedido_Estado");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.SeguimientoPedido)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SeguimientoPedido_Pedido");
        });

        modelBuilder.Entity<TipoEntrega>(entity =>
        {
            entity.HasKey(e => e.IdTipoEntrega).HasName("PK__TipoEntr__38B9138B8DF9B874");

            entity.HasIndex(e => e.Descripcion, "UQ__TipoEntr__298336B62928EEF6").IsUnique();

            entity.Property(e => e.IdTipoEntrega).HasColumnName("idTipoEntrega");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuario__645723A608B3C420");

            entity.HasIndex(e => e.Cedula, "UQ__Usuario__415B7BE5660888DB").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Usuario__AB6E6164204AEA65").IsUnique();

            entity.Property(e => e.IdUsuario).HasColumnName("idUsuario");
            entity.Property(e => e.Apellido1)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido1");
            entity.Property(e => e.Apellido2)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("apellido2");
            entity.Property(e => e.Cedula)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("cedula");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fechaRegistro");
            entity.Property(e => e.IdEstadoUsuario).HasColumnName("idEstadoUsuario");
            entity.Property(e => e.IdRol).HasColumnName("idRol");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.PasswordHash)
                .IsUnicode(false)
                .HasColumnName("passwordHash");
            entity.Property(e => e.PasswordTemporal)
                .IsUnicode(false)
                .HasColumnName("passwordTemporal");

            entity.HasOne(d => d.IdEstadoUsuarioNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdEstadoUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Estado");

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuario)
                .HasForeignKey(d => d.IdRol)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuario_Rol");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
