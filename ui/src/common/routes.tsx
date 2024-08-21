import React, { lazy } from "react";

const Loader = (Component: any) => (props: any) => {
  return (
    <React.Suspense fallback="Loading">
      <Component {...props} />
    </React.Suspense>
  );
};

const Login = Loader(lazy(() => import("../pages/Login/login")));
const ForgotPassword = Loader(
  lazy(() => import("../components/Login/forgot-password/forgot-Password"))
);
const SignUp = Loader(lazy(() => import("../components/Login/signup/signup")));

const Appsidebar = Loader(
  lazy(() => import("../components/common/Layout/appsidebar"))
);
const SO8 = Loader(lazy(() => import("../pages/SO8/so8")));
const ArrivedVehiclePage = Loader(
  lazy(() => import("../pages/Vehicle-Arrived/list"))
);

const routes = [
  { path: "/", element: <Login /> },
  { path: "/forgot-password", element: <ForgotPassword /> },
  { path: "/signup", element: <SignUp /> },
  {
    path: "/dashboard",
    element: <Appsidebar />,
    children: [
      {
        path: "",
        element: <SO8 />,
      },
    ],
  },
  {
    path: "/vehiclearrived",
    element: <Appsidebar />,
    children: [
      {
        path: "",
        element: <ArrivedVehiclePage />,
      },
    ],
  },
];

export default routes;
