import { FilterMatchMode } from "primereact/api";
import { Card } from "primereact/card";
import { Column, ColumnProps } from "primereact/column";
import { DataTable, DataTableValueArray } from "primereact/datatable";
import { Dialog } from "primereact/dialog";
import { Dropdown } from "primereact/dropdown";
import React, { ReactElement } from "react";
import CustomHeaderComponent from "./custom-header-component";

interface Props {
  rowData?: any[];
  cols?: ColumnProps[];
  actionTemplate?: (rowData: any) => JSX.Element;
  title?: string;
  showAdd?: boolean;
  headerGroup?;
  className?: string;
  children?: ReactElement;
  actionMethod?;
  sortDetails?: any;
  setSortDetails?: any;
  idProps?: string;
  rowsPerPage?: number;
  resizeMode?: "fit" | "expand" | undefined;
  selectionMode?: "single" | "multiple" | undefined;
  emptyMessage?: string;
  groupByFields?: string[];
  rowGroupMode?: "rowspan" | "subheader";
}

const DataTableComponent: React.FC<Props> = ({
  rowData,
  cols,
  actionTemplate,
  title,
  showAdd = true,
  className,
  headerGroup,
  children,
  actionMethod,
  sortDetails,
  setSortDetails,
  idProps,
  rowsPerPage = 10,
  resizeMode = "expand",
  selectionMode,
  emptyMessage = "No Records Found",
  groupByFields,
  rowGroupMode = "rowspan",
}) => {
  const [column, setColumn] = React.useState<ColumnProps[] | undefined>(cols);
  const [openAction, setOpenAction] = React.useState(false);
  const [editRows, setEditRows] = React.useState();
  const [selectedRow, setSelectedRow] = React.useState<DataTableValueArray>([]);
  const dt = React.useRef<DataTable<any[]>>(null);
  const rowPerPageOption = [5, 10, 25];
  const [filters, setFilters] = React.useState<Record<string, any>>({
    global: { value: null, matchMode: FilterMatchMode.CONTAINS },
  });

  React.useEffect(() => {
    if (actionMethod) {
      actionMethod.current.rowAction = handleRowAction;
    }
  });

  const handleRowAction = (action, row) => {
    if (action === "edit") {
      setEditRows(row);
      setOpenAction(true);
    }
  };

  const exportCSV = () => {
    actionMethod.current?.exportCSV();
  };

  const headerTemplate = () => (
    <CustomHeaderComponent
      title={title}
      rowData={rowData}
      showAdd={showAdd}
      children={children}
      cols={cols}
      setFilters={setFilters}
      setColumn={setColumn}
      exportCSV={exportCSV}
    />
  );

  const paginatorTemplate = {
    layout: "RowsPerPageDropdown CurrentPageReport PrevPageLink NextPageLink",
    RowsPerPageDropdown: (options) => {
      const dropdownOptions = rowPerPageOption.map((value) => ({
        label: value,
        value,
      }));

      return (
        <div
          style={{
            display: "flex",
            justifyContent: "flex-end",
            alignItems: "center",
          }}
        >
          <span
            className="mx-1"
            style={{ color: "var(--text-color)", userSelect: "none" }}
          >
            Items per page:{" "}
          </span>
          <Dropdown
            value={options.value}
            options={dropdownOptions}
            onChange={options.onChange}
          />
        </div>
      );
    },
    CurrentPageReport: (options) => {
      return (
        <span
          style={{
            color: "var(--text-color)",
            userSelect: "none",
            width: "120px",
            textAlign: "center",
          }}
        >
          {options.first} - {options.last} of {options.totalRecords}
        </span>
      );
    },
  };

  return (
    <Card>
      <DataTable
        value={rowData || []}
        header={headerTemplate}
        // stripedRows
        size="small"
        ref={actionMethod}
        filters={filters}
        showGridlines
        sortField={sortDetails?.field}
        sortOrder={sortDetails?.order}
        onSort={(e) => {
          setSortDetails({
            field: e.sortField,
            order: e.sortOrder,
          });
        }}
        dataKey={idProps}
        selection={selectedRow}
        onSelectionChange={(e) => {
          setSelectedRow(e.value);
        }}
        columnResizeMode={resizeMode}
        headerColumnGroup={headerGroup ? headerGroup(column) : null}
        className={className}
        resizableColumns
        paginator
        scrollable
        scrollHeight="400px"
        rows={rowsPerPage}
        paginatorTemplate={paginatorTemplate}
        paginatorLeft
        // stateStorage="local" // "local" || "memory"
        // stateKey={`${title}-local`}
        emptyMessage={emptyMessage}
        frozenRow={true}
        exportFilename={`${title}`}
        rowGroupMode={rowGroupMode}
        groupRowsBy={groupByFields as any}
      >
        {selectionMode && (
          <Column
            selectionMode={selectionMode}
            headerStyle={{ width: "3rem" }}
          />
        )}
        {column?.map((col) => (
          <Column {...col} />
        ))}
      </DataTable>

      <Dialog
        visible={openAction}
        onHide={() => setOpenAction(false)}
        header={`Edit ${title}`}
        style={{ width: "30vw" }}
        dismissableMask
      >
        {children &&
          React.cloneElement(children, {
            row: editRows,
          })}
      </Dialog>
    </Card>
  );
};

export default DataTableComponent;
