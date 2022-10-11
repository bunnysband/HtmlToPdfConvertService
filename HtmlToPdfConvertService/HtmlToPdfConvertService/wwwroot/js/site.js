function displayBusyIndicator() {
    $('.loading').show();
}

$(document).on('submit', 'form', function () {
    displayBusyIndicator();
});