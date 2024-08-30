import React from "react";
import DataTableComponent from "../../components/common/DataTable/data-table";
import useVehicleArrivedApi from "../../hooks/api/PreSale/vehicle-arrived";
import { actionColumn, columns } from "./column";
import UseSignalRService from "../../hooks/api/SignalR/signalr";
import { useQueryClient } from "react-query";
import { VEHICLE_ARRIVED_KEY } from "../../common/query-key";

const ArrivedVehiclePage = () => {
  const [arrivedVehicle, setArrivedVehicle] = React.useState([]);
  const { useListVehicles } = useVehicleArrivedApi();
  const { data, isLoading } = useListVehicles({
    pagination: {
      maxResults: 1000,
    },
  });
  const actionMethod = React.useRef<any>(null);
  const queryClient = useQueryClient();
  const signalRConnection = UseSignalRService({
    hubUrl: "/connectionHub",
  }); 

  React.useEffect(() => {
    signalRConnection?.on("VehicleUpdated", (updateData: any) => {
      queryClient.setQueryData([VEHICLE_ARRIVED_KEY], (oldData: any) =>
        oldData.map((x) => (x.id === updateData.id ? updateData : x))
      );
    });
  }, [queryClient, signalRConnection]);

  const vehicleData = React.useMemo(() => data, [data]);

  const handleRowAction = (action, row) => {
    actionMethod.current.rowAction(action, row);
  };

  return (
    <React.Fragment>
      <DataTableComponent
        rowData={vehicleData}
        cols={[...columns, ...actionColumn(handleRowAction)]}
        title="Arrived Vehicles"
        actionMethod={actionMethod}
        loading={isLoading}
        idProps="id"
      />
    </React.Fragment>
  );
};

export default ArrivedVehiclePage;
