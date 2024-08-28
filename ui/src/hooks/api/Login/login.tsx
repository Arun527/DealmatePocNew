import React from "react";
import { useMutation } from "react-query";
import apiClient from "../api-client";

const useAuthenticationApi = () => {
  const useLogin = () => {
    return useMutation<any, Error>(async (data: any) => {
      const response = await apiClient.post(
        `/api/employee/login?email=${data?.email}&password=${data?.password}`
      );
      return response?.data;
    });
  };

  const useForgotPassword = () => {
    return useMutation<any, Error>(async (data: any) => {
      const response = await apiClient.post(
        `/api/employee/changepassword?email=${data?.email}&password=${data?.password}`
      );
      return response?.data;
    });
  };

  return React.useMemo(
    () => ({
      useLogin,
      useForgotPassword,
    }),
    []
  );
};

export default useAuthenticationApi;
