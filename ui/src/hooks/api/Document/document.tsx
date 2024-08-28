import axios from "axios";
import React from "react";
import { useMutation } from "react-query";
import { getData } from "../../../common/app-data";
import { environmentalVariable } from "../../../common/env-variables";

const client = axios.create({
  baseURL: environmentalVariable.BASE_URL,
  headers: {
    Authorization: `Bearer ${getData("token")}`,
    "Content-Type": ["multipart/form-data"],
  },
});

const useDocumentServiceApi = () => {
  const useExcelUpload = () => {
    return useMutation<any, Error>(async (formData: any) => {
      const response = await client.post(`/api/vehicle/fileupload`, formData);
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

export default useDocumentServiceApi;
