import React from "react";
import FileUploadComponent from "../../components/common/FileUpload/file-upload";
import useDocumentApi from "../../hooks/api/Document/document";
import { MutateOptions, useQueryClient } from "react-query";
import { useToastContext } from "../../components/common/Dialog/Toast/toast";
import { VEHICLE_ARRIVED_KEY } from "../../common/query-key";

const SO8 = () => {
  const showToast = useToastContext();
  const { useExcelUpload } = useDocumentApi();
  const { mutate: mutateExcelUpload } = useExcelUpload();
  const queryClient = useQueryClient();
  const env = process.env.NODE_ENV;
  console.log(`Environment`,env);
  const callback = () => {
    return {
      onSuccess: (response: any) => {
        if (response) {
          queryClient.invalidateQueries({ queryKey: [VEHICLE_ARRIVED_KEY] });
          showToast({
            severity: "success",
            detail: "File Uploaded Successfully",
          });
        }
      },
      onError: (error: any) => {
        console.log("error", error);
        showToast({
          severity: "error",
          detail: error?.response?.data?.error?.message,
        });
      },
    } as MutateOptions;
  };

  const handleFileUpload = async (formData: any) => {
    await mutateExcelUpload(formData, callback());
  };

  return (
    <div className="flex justify-content-center align-items-center h-screen">
      <FileUploadComponent
        handleFileUpload={handleFileUpload}
        fieldProperties={{
          name: "file",
          buttonLabel: "Upload",
          accept: ".xlsx,.xls,.xlsm,.xlsb,.csv",
          emptyMessage: "Only Upload Excel/CSV file.",
        }}
      />
    </div>
  );
};

export default SO8;
