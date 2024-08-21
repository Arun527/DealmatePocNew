import { Button } from "primereact/button";
import { Checkbox } from "primereact/checkbox";
import { IconField } from "primereact/iconfield";
import { InputIcon } from "primereact/inputicon";
import { InputText } from "primereact/inputtext";
import { OverlayPanel } from "primereact/overlaypanel";
import React from "react";

interface Props {
  title?: string;
  rowData?: any[];
  showAdd?: boolean;
  cols?: any[] | undefined;
  setColumn?: (cols: any[]) => void;
  setFilters?: (filters: any) => void;
  children?: any;
  exportCSV?: () => void;
}

const CustomHeaderComponent: React.FC<Props> = ({
  title,
  rowData = [],
  showAdd,
  cols,
  setColumn,
  setFilters,
  children,
  exportCSV,
}: Props) => {
  const [toolBarCols, setToolBarCols] = React.useState<any[]>();

  React.useEffect(() => {
    setToolBarCols(
      cols?.filter((item) => item.header && item.field !== "action") ?? []
    );
  }, [cols]);

  const [globalFilterValue, setGlobalFilterValue] = React.useState<string>("");

  const op = React.useRef<OverlayPanel>(null);
  const addOverlay = React.useRef<OverlayPanel>(null);

  const onGlobalFilterChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setFilters?.((prevFilters) => ({
      ...prevFilters,
      global: { ...prevFilters.global, value },
    }));
    setGlobalFilterValue(value);
  };

  const handleColumnSettingClick = (e) => {
    op.current?.toggle(e);
  };

  const toggleColumnVisibility = (event, x) => {
    const newColumns = toolBarCols?.map((col) => {
      if (x.field === col.field) {
        return { ...col, hidden: !col.hidden };
      }
      return col;
    });
    setToolBarCols(newColumns);
  };

  const handleColumnSelectionOk = (updatedCols) => {
    const updatedCol = (cols ?? []).map((x, index) => {
      return { ...x, hidden: updatedCols[index]?.hidden ?? false };
    });
    setColumn?.(updatedCol);
    op.current?.hide();
  };

  return (
    <>
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
        }}
      >
        <p style={{ fontWeight: "bold", margin: 0 }}>{title}</p>
        <div style={{ display: "flex", alignItems: "center" }}>
          <IconField iconPosition="left">
            <InputIcon className="pi pi-search" />
            <InputText
              value={globalFilterValue}
              onChange={onGlobalFilterChange}
              placeholder="Search..."
            />
          </IconField>
          <Button
            icon="pi pi-cog"
            rounded
            style={{ marginLeft: ".5em" }}
            onClick={handleColumnSettingClick}
          />
          {showAdd && (
            <Button
              icon="pi pi-plus"
              className="p-button-rounded"
              onClick={(e) => addOverlay.current?.toggle(e)}
              style={{ marginLeft: ".5em" }}
            />
          )}
          <Button
            icon="pi pi-file-excel"
            rounded
            data-pr-tooltip="XLS"
            style={{ marginLeft: ".5em" }}
            onClick={exportCSV}
          />
        </div>
        <OverlayPanel ref={op} dismissable>
          <h4>{"Column Settings"}</h4>
          {toolBarCols
            ?.filter((item) => item.header && item.field !== "action")
            .map((x, i) => (
              <div
                key={i}
                style={{
                  display: "flex",
                  alignItems: "center",
                  marginBottom: "10px",
                }}
              >
                <Checkbox
                  inputId={`col_${i}`}
                  checked={!x.hidden}
                  onChange={(e) => toggleColumnVisibility(e, x)}
                />
                <label htmlFor={`col_${i}`} style={{ marginLeft: "8px" }}>
                  {x?.header || x?.field}
                </label>
              </div>
            ))}
          <div className="card flex justify-content-center">
            <Button
              style={{ marginTop: "10px", width: "210px" }}
              label={"Ok"}
              onClick={() => handleColumnSelectionOk(toolBarCols)}
            />
          </div>
        </OverlayPanel>

        <OverlayPanel ref={addOverlay} dismissable style={{ width: "30%" }}>
          {children ? (
            <>
              <h4>{`Add ${title}`}</h4>
              {React.cloneElement(children)}
            </>
          ) : (
            <div className="card flex justify-content-center">
              <h2>There is no more content...</h2>
            </div>
          )}
        </OverlayPanel>
      </div>
    </>
  );
};

export default CustomHeaderComponent;
