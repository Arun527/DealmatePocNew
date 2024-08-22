import { ColumnProps } from "primereact/column";

export const columns = [
  {
    field: "loadNo",
    header: "Load Number",
    sortable: true,
  },
  {
    field: "frameNo",
    header: "Frame Number",
    sortable: true,
  },
  {
    field: "manufactureDate",
    header: "Manufacture Date",
    sortable: true,
    body: (data) => new Date(data?.manufactureDate).toLocaleDateString(),
  },
  {
    field: "fuelType",
    header: "Fuel Type",
    sortable: true,
  },
  {
    field: "key",
    header: "Key",
    sortable: true,
  },
  {
    field: "mileage",
    header: "Mileage",
    sortable: true,
  },
  {
    field: "createdOn",
    header: "Created At",
    sortable: true,
    body: (data) => new Date(data?.createdOn).toLocaleDateString(),
  },
  {
    field: "updatedOn",
    header: "Updated At",
    sortable: true,
    body: (data) => new Date(data.updatedOn).toLocaleDateString(),
  },
] as ColumnProps[];

export const actionColumn = (handleRowAction) =>
  [
    {
      field: "actions",
      header: "Actions",
      exportable: false,
      headerStyle: { width: "10%", minWidth: "8rem" },
      bodyStyle: { textAlign: "center" },
      body: (data) => (
        <button
          type="button"
          className="btn btn-primary btn-sm"
          onClick={() => handleRowAction("edit", data)}
        >
          Edit
        </button>
      ),
    },
  ] as ColumnProps[];
