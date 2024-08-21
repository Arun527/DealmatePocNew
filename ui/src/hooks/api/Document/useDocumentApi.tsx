import React from "react";
import { useMutation } from "react-query";
import apiClient from "../api-client";

const useDocumentApi = () => {
  const useExcelUpload = () => {
    return useMutation<any, Error>(async (formData: any) => {
      const response = await apiClient.post(
        `/api/vehicle/fileupload`,
        formData
      );
      return response?.data;
    });
  };

  return React.useMemo(
    () => ({
      useExcelUpload,
    }),
    []
  );
};

export default useDocumentApi;
