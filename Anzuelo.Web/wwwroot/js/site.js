// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function obtenerTokenAntiforgery() {
    const input = document.querySelector('#antiforgeryGlobalForm input[name="__RequestVerificationToken"]');
    return input ? input.value : '';
}

function agregarAlCarritoDesdeCatalogo(idProducto, idCombo, cantidad, mensajes) {
    if (!cantidad || cantidad <= 0) {
        Swal.fire({ title: mensajes.error, text: mensajes.cantidadInvalida, icon: "error" });
        return;
    }

    const params = new URLSearchParams();
    if (idProducto) params.append('idProducto', idProducto);
    if (idCombo) params.append('idCombo', idCombo);
    params.append('cantidad', cantidad);
    params.append('observaciones', '');

    fetch('/Pedido/AgregarLinea', {
        method: 'POST',
        headers: {
            'RequestVerificationToken': obtenerTokenAntiforgery(),
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: params
    })
        .then(response => {
            if (!response.ok) return response.text().then(text => { throw new Error(text) });
            return response.text();
        })
        .then(() => {
            if (typeof actualizarContadorCarrito === 'function') {
                actualizarContadorCarrito();
            }
            Swal.fire({
                title: mensajes.exito,
                icon: "success",
                showConfirmButton: false,
                timer: 1200
            });
        })
        .catch(error => {
            Swal.fire({ title: mensajes.error, text: error.message, icon: "error" });
        });
}