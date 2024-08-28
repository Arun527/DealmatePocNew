import React from "react";
import { useQuery } from "react-query";
import apiClient from "../api-client";
import { VEHICLE_ARRIVED_KEY } from "../../../common/query-key";

const useVehicleArrivedApi = () => {
  const useListVehicles = () => {
    return useQuery<any, Error>([VEHICLE_ARRIVED_KEY], async () => {
      const response = await apiClient.post(`/api/vehicle/list`, {});
      return response?.data;
    });
  };

  return React.useMemo(
    () => ({
      useListVehicles,
    }),
    []
  );
};

export default useVehicleArrivedApi;
