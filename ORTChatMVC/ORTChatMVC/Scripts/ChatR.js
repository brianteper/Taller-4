$(function () {
    var myConnection = $.connection.chatHub;

    $.connection.hub.start();

    myConnection.client.addChatMessage = function (data) {
        var info = JSON.parse(data);
        $("#messages").append("<li>" + info.name + ': ' + info.message + "</li>");
    };

    $('#send').click(function () {
        var myName = $("#name").val();
        var myMessage = $("#message").val();
        $("#message").val('');
        myConnection.server.pushMessageToClients(JSON.stringify({ name: myName, message: myMessage }));
    });

    $('#clear').click(function () {
        $('#messages li').remove();
    });
});