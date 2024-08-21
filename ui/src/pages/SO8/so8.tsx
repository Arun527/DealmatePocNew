import React from "react";
import FileUploadComponent from "../../components/common/FileUpload/file-upload";
import useDocumentApi from "../../hooks/api/Document/useDocumentApi";
import { MutateOptions } from "react-query";
import { useToastContext } from "../../components/common/Dialog/Toast/toast";

const SO8 = () => {
  const showToast = useToastContext();
  const { useExcelUpload } = useDocumentApi();
  const { mutate: mutateExcelUpload } = useExcelUpload();

  const callback = () => {
    return {
      onSuccess: (response: any) => {
        console.log("success", response);
        if (response) {
          showToast({
            severity: "success",
            detail: "File Uploaded Successfully",
          });
        }
      },
      onError: (error: any) => {
        console.log("error", error);
        showToast({ severity: "error", detail: error?.message });
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
