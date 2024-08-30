import React from "react";
import * as signalR from "@microsoft/signalr";

interface Props {
  hubUrl: string;
}

const UseSignalRService = ({ hubUrl }: Props) => {
  const [connection, setConnection] = React.useState<signalR.HubConnection>();

  React.useEffect(() => {
    const newConnection = new signalR.HubConnectionBuilder()
      .withUrl(process.env.REACT_APP_API_BASE_URL + hubUrl)
      .withAutomaticReconnect({ nextRetryDelayInMilliseconds: () => 50000 })
      .configureLogging(signalR.LogLevel.Information)
      .build();

    setConnection(newConnection);
    newConnection
      .start()
      .then(() => console.log("SignalR Connected."))
      .catch((err) => console.log("Error connecting SignalR: ", err));
  }, [hubUrl]);

  return connection;
};

export default UseSignalRService;
