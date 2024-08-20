import { Message } from "primereact/message";
import React from "react";
interface Props {
  message: string;
  unstyled?: boolean;
}

const ErrorMessage = ({ unstyled = true, message = "" }: Props) => {
  const text = message.charAt(0).toUpperCase() + message.slice(1);

  return (
    <div style={{ display: message ? "block" : "none", marginTop: 1 }}>
      <Message
        severity="error"
        text={text}
        style={{
          padding: "3px",
          backgroundColor: unstyled ? "unset" : "",
          color: "red",
        }}
        className="error-text"
        pt={{ text: { style: { fontSize: "14px" } } }}
        icon={unstyled ? <React.Fragment /> : undefined}
      />
    </div>
  );
};

export default ErrorMessage;
