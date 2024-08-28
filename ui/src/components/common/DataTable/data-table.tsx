import { FilterMatchMode } from "primereact/api";
import { Card } from "primereact/card";
import { Column, ColumnProps } from "primereact/column";
import { DataTable, DataTableValueArray } from "primereact/datatable";
import { Dialog } from "primereact/dialog";
import { Dropdown } from "primereact/dropdown";
import React, { ReactElement } from "react";
import CustomToolbar from "./custom-toolbar";

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
  idProps?: string;
  rowsPerPage?: number;
  resizeMode?: "fit" | "expand" | undefined;
  selectionMode?: "single" | "multiple" | undefined;
  emptyMessage?: string;
  groupByFields?: string[];
  rowGroupMode?: "rowspan" | "subheader";
  size?: "small" | "large" | "normal" | undefined;
  isGridLines?: boolean;
  loading?: boolean;
  isReOrderColumns?: boolean;
  showPagination?: boolean;
}

const DataTableComponent = ({
  rowData = [],
  cols = [],
  actionTemplate,
  title,
  showAdd = true,
  className,
  headerGroup,
  children,
  actionMethod,
  sortDetails,
  idProps,
  rowsPerPage = 10,
  resizeMode = "expand",
  selectionMode,
  emptyMessage = "No Records Found",
  groupByFields,
  rowGroupMode = "rowspan",
  size = "small",
  isGridLines = true,
  loading,
  isReOrderColumns = false,
  showPagination = true,
}: Props) => {
  const [column, setColumn] = React.useState<ColumnProps[]>(cols);
  const [openAction, setOpenAction] = React.useState(false);
  const [editRows, setEditRows] = React.useState();
  const [selectedRow, setSelectedRow] = React.useState<DataTableValueArray>([]);
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
    const selectedRowLength = selectedRow.length;
    actionMethod?.current?.exportCSV({
      selectionOnly: Boolean(selectedRowLength),
    });
  };

  const headerTemplate = () => (
    <CustomToolbar
      title={title}
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
        dataKey={idProps}
        value={rowData}
        header={headerTemplate}
        size={size}
        ref={actionMethod}
        filters={filters}
        showGridlines={isGridLines}
        stripedRows
        loading={loading}
        sortField={sortDetails?.field}
        sortOrder={sortDetails?.order}
        columnResizeMode={resizeMode}
        reorderableColumns={isReOrderColumns}
        selection={selectedRow}
        onSelectionChange={(e) => {
          setSelectedRow(e.value);
        }}
        headerColumnGroup={headerGroup ? headerGroup(column) : null}
        className={className}
        resizableColumns
        paginator
        scrollable
        scrollHeight="400px"
        rows={rowsPerPage}
        paginatorTemplate={paginatorTemplate}
        paginatorLeft
        emptyMessage={emptyMessage}
        exportFilename={`${title}`}
        rowGroupMode={rowGroupMode}
        groupRowsBy={groupByFields as any}
        alwaysShowPaginator={showPagination}
      >
        {
          // selectionMode details
          selectionMode && (
            <Column
              selectionMode={selectionMode}
              headerStyle={{ width: "3rem" }}
            />
          )
        }
        {column?.map((col: any) => (
          <Column
            {...col}
            body={col.field === "action" ? actionTemplate : col.body}
            exportable={col.exportable ?? !col.hidden}
          />
        ))}
      </DataTable>

      <Dialog
        visible={openAction}
        onHide={() => setOpenAction(false)}
        header={`Edit ${title}`}
        style={{ width: "30vw" }}
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
