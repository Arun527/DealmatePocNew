import React from "react";
import FileUploadComponent from "../../components/common/FileUpload/file-upload";

const SO8 = () => {
  const handleFileUpload = (event: any) => {
    console.log(event);
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
