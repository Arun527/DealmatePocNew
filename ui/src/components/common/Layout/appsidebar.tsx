import { BreadCrumb } from "primereact/breadcrumb";
import { Menubar } from "primereact/menubar";
import { PanelMenu } from "primereact/panelmenu";
import { Sidebar } from "primereact/sidebar";
import React from "react";
import { Outlet, useLocation } from "react-router-dom";
import { headerData, sidebarData } from "../../data/sidebard-data";

export const drawerWidth = 80;
export const drawerWidthExpand = 285;

export default function Appsidebar() {
  const [visible, setVisible] = React.useState(true);

  const location = useLocation();
  const pathname: any = location.pathname?.split("/")?.pop();

  const breadcrumbItems = [
    { label: "Home", url: "/" },
    {
      label: pathname.charAt(0).toUpperCase() + pathname.slice(1),
      url: `/${pathname}`,
    },
  ];

  const menubarItems = headerData.map((item) => ({
    ...item,
    command: () => (item.label === "" ? setVisible(!visible) : null),
  }));

  return (
    <>
      <div
        style={{
          paddingLeft: !visible ? 20 + "px" : drawerWidthExpand + 20 + "px",
        }}
        className="card custom-menubar-container"
      >
        <Menubar
          model={menubarItems}
          style={{ display: "flex", justifyContent: "space-between" }}
        />
        <BreadCrumb
          model={breadcrumbItems}
          separatorIcon="pi pi-minus"
          pt={{
            separatorIcon: { style: { rotate: "125deg" } },
          }}
        />
      </div>
      <Sidebar
        visible={visible}
        transitionOptions={{ timeout: 0 }}
        baseZIndex={0}
        showCloseIcon={false}
        header="DealMate"
        pt={{
          header: { style: { color: "red", height: "100px" } },
          content: { style: { padding: 0 } },
          mask: { style: { animation: "unset", maxWidth: "20%" } },
        }}
        onHide={() => setVisible(true)}
      >
        <PanelMenu
          model={sidebarData}
          transitionOptions={{ timeout: 0 }}
          pt={{
            menu: {
              style: { marginBottom: "5px", border: "none" },
            },
            headerContent: { style: { border: "none" } },
            menuitem: {
              className: "sidebar-menuitem",
              style: {
                marginBottom: "5px",
                cursor: "pointer",
                border: "none",
              },
            },
            menuContent: { style: { padding: "5px" } },
          }}
        />
      </Sidebar>
      <div
        style={{
          paddingLeft: visible ? drawerWidthExpand + 20 + "px" : 20 + "px",
          paddingTop: "110px",
        }}
      >
        <Outlet />
      </div>
    </>
  );
}
