// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.

function toastSuccess(message)
{

    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 10000,
        timerProgressBar: true
    })

    Toast.fire({
        icon: 'success',
        title: message
    })
}

function toastError(message) {

    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 10000,
        timerProgressBar: true
    })

    Toast.fire({
        icon: 'error',
        title: message
    })
}

function toastWarning(message) {

    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 10000,
        timerProgressBar: true
    })

    Toast.fire({
        icon: 'warning',
        title: message
    })
}

function toastInfo(message) {

    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 10000,
        timerProgressBar: true
    })

    Toast.fire({
        icon: 'info',
        title: message
    })
}

function toastQuestion(message) {

    const Toast = Swal.mixin({
        toast: true,
        position: 'bottom-end',
        showConfirmButton: false,
        timer: 10000,
        timerProgressBar: true
    })

    Toast.fire({
        icon: 'question',
        title: message
    })
}
