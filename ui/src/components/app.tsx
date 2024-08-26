import React from "react";
import { useLocation, useNavigate, useRoutes } from "react-router-dom";
import routes from "../common/routes";
import { getData } from "../common/app-data";

const App = () => {
  const { pathname } = useLocation();
  const navigate = useNavigate();
  const token = getData("token");
  const loggedIn = Boolean(token);
  const pages = useRoutes(routes);
  const previousPathname = React.useRef("");

  React.useEffect(() => {
    if (loggedIn) {
      navigate(pathname == "/" ? "/dashboard" : pathname);
    } else {
      navigate("/");
    }

    previousPathname.current = pathname;
  }, [loggedIn, pathname]);

  React.useEffect(() => {
    const authPages = ["/", "/signup", "/forgot-password"];
    const root = document.getElementById("root");
    if (authPages.includes(pathname)) {
      root?.classList.add("auth-page");
    } else {
      root?.classList.remove("auth-page");
    }
  }, [pathname]);

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
  }, [loggedIn, pathname]);

  return <div className="App">{pages}</div>;
};

export default App;
