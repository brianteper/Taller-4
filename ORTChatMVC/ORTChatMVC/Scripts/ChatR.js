$(function () {
    var myConnection = $.connection.chatHub;

    $.connection.hub.start();

    myConnection.client.addChatMessage = function (data) {
        var info = JSON.parse(data);
        $('#messages').append('<li><strong>' + info.name + '</strong>: ' + info.message + '</li>');
    };

    myConnection.client.showIsTyping = function (data) {
        var info = JSON.parse(data);

        var found = false;
        $("#isTyping li").each(function () {
            if ($(this).data("id") == info.connectionId) {
                found = true;
            }
        });

        if (!found) {
            $('#isTyping').append('<li data-id=' + info.connectionId + '>' + info.name + ' is typing...</li>');
        }
    };

    myConnection.client.hideIsTyping = function (data) {
        var info = JSON.parse(data);

        var found = false;
        $("#isTyping li").each(function () {
            if ($(this).data("id") == info.connectionId) {
                $(this).remove();
            }
        });
    }

    $('#send').click(function () {
        var myName = $('#name').val();
        var myMessage = $('#message').val();
        $('#message').val('');
        myConnection.server.pushMessageToClients(JSON.stringify({ name: myName, message: myMessage }));
    });

    $('#message').keypress(function (e) {
        if (e.which == 13) {
            $('#send').click();
        }
    });

    $('#clear').click(function () {
        $('#messages li').remove();
    });

    $('#message').focus(function () {
        if ($('#name').val() == "") {
            $('#name').focus();
            $('#nameError').show();
        } else {
            var myName = $('#name').val();
            myConnection.server.someoneIsTyping(JSON.stringify({ name: myName }));
        }
    });

    $('#message').blur(function () {
        if ($('#name').val() != "") {
            myConnection.server.finishTyping();
        }
    });

    $('#name').blur(function () {
        $('#nameError').hide();
    });
});