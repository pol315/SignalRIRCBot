<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SignalRTest1.index" %>

<!DOCTYPE html>
<html>
<head>
    <title>SignalR IRC Chat</title>
    <style type="text/css">
        .container {
            background-color: #99CCFF;
            border: thick solid #808080;
            padding: 20px;
            margin: 20px;
        }
    </style>

    <script src="scripts/jquery-1.6.4.min.js"></script>
    <script src="scripts/jquery.signalR-2.2.0.min.js"></script>
    <script src="signalr/hubs"></script>   
</head>
<body>
    <div class="container">        
            <input type="text" id="message" />
            <input type="button" id="sendmessage" value="Send" />
            <input type="hidden" id="displayname" />
        <ul id="discussion"></ul>
    </div>

    <script type="text/javascript">
        $(function () {
            // Declare a proxy to reference the hub.
            var chat = $.connection.chatHub;
            // Create a function that the hub can call to broadcast messages.
            chat.client.broadcastMessage = function (name, message) {
                // Html encode display name and message.
                var encodedName = $('<div />').text(name).html();
                var encodedMsg = $('<div />').text(message).html();
                // Add the message to the page.
                $('#discussion').append('<li><strong>' + encodedName
                    + '</strong>:&nbsp;&nbsp;' + encodedMsg + '</li>');
            };
            // Get the user name and store it to prepend to messages.
            //$('#displayname').val(prompt('Enter your name:', ''));
            $('#displayname').val("temp");
            // Set initial focus to message input box.
            $('#message').focus();
            // Start the connection.
            $.connection.hub.start().done(function () {
                $('#sendmessage').click(function () {
                    // Call the Send method on the hub.
                    // chat.server.send($('#displayname').val(), $('#message').val());
                    $.ajax({
                        url: "index.aspx/IRCWrite",
                        type: 'POST',
                        data: "{'message': '" + $("#message").val() + "'}",
                        contentType: "application/json; charset=utf-8",
                        dataType: 'json',
                        success: function (data, status) {
                            chat.server.send($('#displayname').val(), $('#message').val());
                            $('#message').val('').focus();
                        }                        
                    });
                    // Clear text box and reset focus for next comment.
                    
                });
            });
        });
    </script>
    
    
</body>
</html>
