<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="SignalRTest1.index" %>

<!DOCTYPE html>
<html>
<head>
    <link href="css/bootstrap.min.css" rel="stylesheet">
    <link href="css/bootstrap-theme.min.css" rel="stylesheet">
    <link href="css/dashboard.css" rel="stylesheet">
    <title>SignalR IRC Chat</title>
    <!-- <style type="text/css">
        .container {
            background-color: #99CCFF;
            border: thick solid #808080;
            padding: 20px;
            margin: 20px;
        }
    </style> -->

    <script src="scripts/jquery-1.6.4.min.js"></script>
    <script src="scripts/jquery.signalR-2.2.0.min.js"></script>
    <script src="signalr/hubs"></script>   
</head>
<body>

    <nav class="navbar navbar-inverse navbar-fixed-top">
      <div class="container-fluid">
        <div class="navbar-header">
          <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar" aria-expanded="false" aria-controls="navbar">
            <span class="sr-only">Toggle navigation</span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
            <span class="icon-bar"></span>
          </button>
          <a class="navbar-brand" href="#">SignalR IRC Realtime Chat</a>
        </div>
        <div id="navbar" class="navbar-collapse collapse">
          <!-- <ul class="nav navbar-nav navbar-right">
            <li><a href="#">Dashboard</a></li>
            <li><a href="#">Settings</a></li>
            <li><a href="#">Profile</a></li>
            <li><a href="#">Help</a></li>
          </ul>
          <form class="navbar-form navbar-right">
            <input type="text" class="form-control" placeholder="Search...">
          </form> -->
        </div>
      </div>
    </nav>

    <div class="container-fluid">
      <div class="row">
        <div class="col-sm-3 col-md-2 sidebar">
          <ul class="nav nav-sidebar" id="users">
          
          </ul>
        </div>
        <div class="col-sm-9 col-sm-offset-3 col-md-10 col-md-offset-2 main">
          <h1 class="page-header"><%= tIRC.IRCChannel %> </h1>

          <div class="col-xs-6">
          <div class="row placeholders">
              <div class="col-xs-4">
                    <input type="text" class="form-control" id="message" placeholder="Message...">
              </div>
              <div class="col-xs-2">
                    <input type="button" id="sendmessage" value="Send" />
                    <input type="hidden" id="displayname" />
              </div>
                
                
                
          </div>

            <div class="row">
                <ul id="discussion">

                </ul>  
            </div>
          </div>
        </div>
      </div>
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

            chat.client.broadcastUser = function (user) {
                // Html encode display name and message.
                var encodedUser = $('<div />').text(user).html();
                
                // Add the message to the page.
                $('#users').append('<li>' + encodedUser + '</li>');
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
