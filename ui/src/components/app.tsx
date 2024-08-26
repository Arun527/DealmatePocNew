import React from "react";
import { useLocation, useNavigate, useRoutes } from "react-router-dom";
import routes from "../common/routes";
import { getData } from "../common/app-data";

const App = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const token = getData("token");
  const loggedIn = Boolean(token);
  const pages = useRoutes(routes);
  const previousPathname = React.useRef("");

  React.useEffect(() => {
    if (!loggedIn && location?.pathname === "/") {
      navigate("/");
    } else {
      navigate(
        loggedIn && location?.pathname === "/"
          ? "/dashboard"
          : location.pathname
      );
    }
    previousPathname.current = location.pathname;
  }, [loggedIn, location.pathname]);

  React.useEffect(() => {
    const authPages = ["/", "/signup", "/forgot-password"];
    const root = document.getElementById("root");
    if (authPages.includes(location?.pathname)) {
      root?.classList.add("auth-page");
    } else {
      root?.classList.remove("auth-page");
    }
  }, [location?.pathname]);

  React.useEffect(() => {
    const handleBrowserBackArrow = () => {
      if (previousPathname.current === "/" && loggedIn) {
        navigate("/dashboard");
      }
    };
    window.addEventListener("popstate", handleBrowserBackArrow);
    return () => {
      window.removeEventListener("popstate", handleBrowserBackArrow);
    };
  }, [loggedIn, location.pathname]);

  return <div className="App">{pages}</div>;
};

export default App;
