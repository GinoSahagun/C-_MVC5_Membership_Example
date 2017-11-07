$(function () {
    var adminMenu = $('[data-admin-menu]');
    adminMenu.hover(function () {
        adminMenu.toggleClass('open');
    });
    
});