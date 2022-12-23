// import * as WebSocket from 'ws';
// use websocket library from npm instead of ws
import { server as WSServer } from "websocket";
import * as http from "http";

const port = 8080;

const server = http.createServer();
server.listen(port);

const wss = new WSServer({
  httpServer: server,
});
  
// create server

  

// wss.on('connection', (ws: WebSocket) => {
//   ws.on('message', (message: string) => {
//     // process incoming message and store values
//     wss.clients.forEach((client) => {
//       if (client !== ws && client.readyState === WebSocket.OPEN) {
//         // send message to all connected controllers and the game
//         client.send(message);
//       }
//     });
//   });
// });

// I'm maintaining all active connections in this object
const clients: any = {};

// This code generates unique userid for everyuser.
const getUniqueID = () => {
  const s4 = () => Math.floor((1 + Math.random()) * 0x10000).toString(16).substring(1);
  return s4() + s4() + '-' + s4();
};

wss.on('request', function(request) {
  var userID = getUniqueID();
  console.log((new Date()) + ' Recieved a new connection from origin ' + request.origin + '.');
  // You can rewrite this part of the code to accept only the requests from allowed origin
  const connection = request.accept(null, request.origin);
  clients[userID] = connection;
  console.log('connected: ' + userID + ' in ' + Object.getOwnPropertyNames(clients))

  connection.on('message', function(message) {
    if (message.type === 'utf8') {
      console.log('Received Message: ' + message.utf8Data);
      const messageJson = JSON.parse(message.utf8Data);
      const now = new Date().getTime();
      const timeDiff = now - messageJson.timestamp;
      console.log('latency: ', timeDiff, 'ms');
      // // process incoming message and store values
      // for (var key in clients) {
      //   clients[key].sendUTF(message.utf8Data);
      //   console.log('sent Message to: ', clients[key])
      // }
    }
  });
});

console.log(`Server started on port ${port}`);

// https://github.com/AvanthikaMeenakshi/node-websockets/blob/master/server/index.js
// https://blog.logrocket.com/websockets-tutorial-how-to-go-real-time-with-node-and-react-8e4693fbf843/
