import React from 'react';
import './App.css';
import { w3cwebsocket as W3CWebSocket } from "websocket";

interface Props { }

interface State { }

class App extends React.Component<Props, State> {
  private socket?: W3CWebSocket = undefined;

  componentDidMount() {
    this.socket = new W3CWebSocket('ws://127.0.0.1:8080');
    this.socket.onopen = () => {
      console.log('WebSocket Client Connected');
    };
  }

  componentWillUnmount() {
    this.socket?.close();
  }

  render() {
    const sendMsg = () => {
      this.socket?.send(JSON.stringify({
        type: "contentchange",
        username: "asfasdf",
        content: "Asfasf223",
        timestamp: new Date().getTime()
      }));
    };

    return (
      <div>
        <button onClick={sendMsg}>Send message</button>
      </div>
    );
  }
}

export default App;