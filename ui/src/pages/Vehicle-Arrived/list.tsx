import React from "react";
import DataTableComponent from "../../components/common/DataTable/data-table";
import useVehicleArrivedApi from "../../hooks/api/PreSale/vehicle-arrived";
import { actionColumn, columns } from "./column";

const ArrivedVehiclePage = () => {
  const [arrivedVehicle, setArrivedVehicle] = React.useState([]);
  const { useListVehicle } = useVehicleArrivedApi();
  const { isLoading, data } = useListVehicle();
  const actionMethod = React.useRef<any>(null);
   
  React.useEffect(() => {
    if (data) {
      setArrivedVehicle(data);
    }
  }, [data]);

  const handleRowAction = (action, row) => {
    actionMethod.current.rowAction(action, row);
  };

  return (
    <React.Fragment>
      <DataTableComponent
        rowData={arrivedVehicle}
        cols={[...columns, ...actionColumn(handleRowAction)]}
        title="Arrived Vehicles"
        actionMethod={actionMethod}
        idProps="id"
      />
    </React.Fragment>
  );
};

export default ArrivedVehiclePage;
