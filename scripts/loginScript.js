$("#loginButtonn").click(function (event) {
    event.preventDefault();

    $('form').fadeOut(500);
    $('.login-wrapper').addClass('form-success');
});