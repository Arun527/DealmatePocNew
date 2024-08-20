import React from "react";
import { useRoutes } from "react-router-dom";
import routes from "../common/routes";

const App = () => {
  const pages = useRoutes(routes);

  React.useEffect(() => {
    const authPages = ["/", "/signup", "/forgot-password"];
    const root = document.getElementById("root");
    if (authPages.includes(window.location.pathname)) {
      root?.classList.add("auth-page");
    } else {
      root?.classList.remove("auth-page");
    }
  }, [window.location.pathname]);

  return <div className="App">{pages}</div>;
};

export default App;
