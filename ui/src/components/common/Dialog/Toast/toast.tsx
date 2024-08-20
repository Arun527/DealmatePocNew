import { Toast, ToastMessage } from "primereact/toast";
import React from "react";

export const ToastContext = React.createContext<Function>(
  (toastMessage: ToastMessage) => null
);

export const ToastContextProvider = ({ children }) => {
  const toastRef = React.useRef<Toast>(null);

  const showToast = (toastMessage: ToastMessage) => {
    if (!toastMessage?.life) {
      toastMessage.life = 5000;
    }
    if (toastRef.current) {
      toastRef.current.show(toastMessage);
    }
  };

  return (
    <ToastContext.Provider value={showToast}>
      <Toast
        ref={toastRef}
        pt={{
          icon: { style: { width: "1.5rem", height: "1.5rem" } },
          detail: { style: { margin: "0px" } },
        }}
      />
      {children}
    </ToastContext.Provider>
  );
};

export const useToastContext = () => React.useContext(ToastContext);
