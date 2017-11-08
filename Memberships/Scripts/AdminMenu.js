$(function () {
    //Admin Menu is Opened Through Clicking 
    var adminMenu = $('[data-admin-menu]');
    adminMenu.click((event) => {
        event.stopPropagation();
        adminMenu.toggleClass('open');
    });
    //Admin Menu is Opened User Clicks on Document and closes menu
    $(document).click(() => {
        if (adminMenu.hasClass('open'))
            adminMenu.removeClass('open');
    });

});