import { BreadCrumb } from "primereact/breadcrumb";
import { Menubar } from "primereact/menubar";
import { PanelMenu } from "primereact/panelmenu";
import { Sidebar } from "primereact/sidebar";
import React from "react";
import { Outlet, useLocation, useNavigate } from "react-router-dom";
import { headerData, sidebarData } from "../../data/sidebard-data";
import { clearData } from "../../../common/app-data";

export const drawerWidth = 80;
export const drawerWidthExpand = 285;

export default function AppSidebar() {
  const navigate = useNavigate();
  const location = useLocation();
  const [sidebarVisible, setSidebarVisible] = React.useState(true);
  const [expandedKeys, setExpandedKeys] = React.useState<string[]>([]);

  const pathname = location.pathname?.split("/")?.pop() || "";

  const navigateTo = (path: string) => {
    if (path) {
      navigate(path);
    }
  };

  const toggleSidebarMenu = (key: string) => {
    setExpandedKeys((prev) =>
      prev.includes(key) ? prev.filter((id) => id !== key) : [key]
    );
  };

  const breadcrumbNavigation = [
    { label: "Home", url: "/" },
    {
      label: pathname.charAt(0).toUpperCase() + pathname.slice(1),
      url: `/${pathname}`,
    },
  ];

  const headerMenuItems = headerData.map((item: any) => ({
    ...item,
    command: () => {
      navigateTo(item?.navigator);
      if (item.label === "") setSidebarVisible(!sidebarVisible);
    },
    items: item?.items?.map((subItem: any) => ({
      ...subItem,
      command: () => {
        if (subItem.label === "Logout") clearData();
        navigateTo(subItem.navigator);
      },
    })),
  }));

  const sidebarModel = React.useMemo(
    () =>
      sidebarData.map((item: any) => ({
        ...item,
        expanded: expandedKeys.includes(item.id),
        command: () => toggleSidebarMenu(item.id),
        items: item?.items?.map((subItem: any) => ({
          ...subItem,
          command: () => navigateTo(subItem.navigator),
        })),
      })),
    [expandedKeys]
  );

  return (
    <React.Fragment>
      <div
        style={{
          paddingLeft: !sidebarVisible ? "20px" : `${drawerWidthExpand + 20}px`,
        }}
        className="card custom-menubar-container"
      >
        <Menubar
          model={headerMenuItems}
          style={{ display: "flex", justifyContent: "space-between" }}
        />
        <BreadCrumb
          model={breadcrumbNavigation}
          separatorIcon="pi pi-minus"
          pt={{
            separatorIcon: { style: { rotate: "125deg" } },
          }}
        />
      </div>
      <Sidebar
        visible={sidebarVisible}
        transitionOptions={{ timeout: 0 }}
        baseZIndex={0}
        showCloseIcon={false}
        header="DealMate"
        pt={{
          header: { style: { color: "red", height: "100px" } },
          content: { style: { padding: 0 } },
          mask: { style: { animation: "unset", maxWidth: "20%" } },
        }}
        onHide={() => setSidebarVisible(true)}
      >
        <PanelMenu
          model={sidebarModel}
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
          paddingLeft: sidebarVisible ? `${drawerWidthExpand + 20}px` : "20px",
          paddingTop: "110px",
        }}
      >
        <Outlet />
      </div>
    </React.Fragment>
  );
}
