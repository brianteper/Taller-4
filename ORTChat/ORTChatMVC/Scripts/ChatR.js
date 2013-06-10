$(function () {
    // Linkeamos la conexión a nuestro Hub en .NET
    var myConnection = $.connection.chatHub;

    // Iniciamos la conexión del cliente
    $.connection.hub.start();

    // Attacheamos el evento 'addChatMessage' para cuando nos envían un mensaje del server se muestre
    myConnection.client.addChatMessage = function (data) {
        var info = JSON.parse(data);
        $('#messages').append('<li><strong>' + info.name + '</strong>: ' + info.message + '</li>');
    };

    // Attacheamos el evento 'showIsTyping' para cuando otro usuario comienza a escribir
    myConnection.client.showIsTyping = function (data) {
        var info = JSON.parse(data);

        var found = false;
        // Validamos si ya existe un ítem con el connectionId para no mostrarlo dos veces
        $("#isTyping li").each(function () {
            if ($(this).data("id") === info.connectionId) {
                found = true;
            }
        });

        // Si no hay un ítem, lo mostramos
        if (!found) {
            $('#isTyping').append('<li data-id=' + info.connectionId + '>' + info.name + ' is typing...</li>');
        }
    };

    // Attacheamos el evento 'hideIsTyping' para cuando otro usuario deja de escribir
    myConnection.client.hideIsTyping = function (data) {
        var info = JSON.parse(data);

        var found = false;
        //Buscamos el elemento que tenga el mismo connectionId y lo eliminamos
        $("#isTyping li").each(function () {
            if ($(this).data("id") === info.connectionId) {
                $(this).remove();
            }
        });
    };

    // Attacheamos el evento 'click' del boton Send para enviar el mensaje al servidor
    $('#send').click(function () {
        var myName = $('#name').val();
        var myMessage = $('#message').val();
        $('#message').val('');
        myConnection.server.pushMessageToClients(JSON.stringify({ name: myName, message: myMessage }));
    });

    // Función para que al apretar Enter se envíe el mensaje
    $('#message').keypress(function (e) {
        if (e.which === 13) {
            var myMessage = $('#message').val();
            if (myMessage !== "") {
                $('#send').click();
            }
        }
    });

    // Función para limpiar la conversación del chat
    $('#clear').click(function () {
        $('#messages li').remove();
    });

    // Al hacer foco en el campo para ingresar un mensaje, enviamos al servidor que el usuario comenzó a escribir
    $('#message').focus(function () {
        // Si no tiene nombre ingresado, mostramos error
        if ($('#name').val() === "") {
            $('#name').focus();
            $('#nameError').show();
        } else {
            var myName = $('#name').val();
            myConnection.server.someoneIsTyping(JSON.stringify({ name: myName }));
        }
    });

    // Al perder el foco en el campo para ingresar un mensaje, enviamos al servidor que el usuario dejó de escribir
    $('#message').blur(function () {
        if ($('#name').val() !== "") {
            myConnection.server.finishTyping();
        }
    });

    // Al perder el foco en el campo Nombre, ocultamos el mensaje de error
    $('#name').blur(function () {
        $('#nameError').hide();
    });
});