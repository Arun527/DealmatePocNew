import { PrimeIcons } from "primereact/api";
import { FileUpload, FileUploadSelectEvent } from "primereact/fileupload";
import { Image } from "primereact/image";
import React from "react";
import { fileValidator } from "./file-validator";

interface FieldPropertiesProps {
  name: string;
  buttonLabel?: string;
  accept: string;
  previewWidth?: number;
  preview?: boolean;
  emptyMessage: string;
  draggable?: boolean;
}

interface FileUploadProps {
  handleFileUpload: (formData: FormData) => void;
  fieldProperties: FieldPropertiesProps;
}

const FileUploadComponent = ({
  handleFileUpload,
  fieldProperties,
}: FileUploadProps) => {
  const {
    name,
    buttonLabel = "Upload",
    accept,
    preview = true,
    previewWidth = 50,
    emptyMessage,
    draggable = true,
  } = fieldProperties;
  const [selectedFile, setSelectedFile] = React.useState<any>(null);
  const [error, setError] = React.useState("");
  const ref = React.useRef<FileUpload>(null);
  const nonImgFileXtn = "xlsx,xls,xlsm,xlsb,csv,docx,pdf,txt,rtf,docx,doc,pdf";

  const handleSelectFile = (e: FileUploadSelectEvent) => {
    clearSelectedFile();
    const file = e.files?.[0];
    if (Boolean(file.name)) {
      const { isValid, message } = fileValidator(file, accept);
      setSelectedFile(isValid ? file : null);
      setError(!isValid ? message : "");
      if (isValid) {
        const formData = new FormData();
        formData.append("file", file);
        handleFileUpload(formData);
      }
    }
  };

  const clearSelectedFile = () => {
    setSelectedFile(null);
    setError("");
  };

  const messageTemplate = (errMsg: string, emptyMsg = "") => (
    <p className={`text-sm ml-2 ${errMsg ? "text-red-500" : ""}`}>
      {!draggable && errMsg
        ? errMsg
        : draggable
        ? dragDropTemplate(errMsg)
        : emptyMsg}
    </p>
  );

  const customPreviewTemplate = (file: any, option: any, errMsg: string) => {
    const fileXtn = file.name?.split(".").pop();
    const imgWH = option?.props?.previewWidth;
    const icon =
      fileXtn === "pdf" ? PrimeIcons.FILE_PDF : PrimeIcons.FILE_EXCEL;
    const show = nonImgFileXtn.split(",").some((x) => x === fileXtn);

    return !Boolean(errMsg) ? (
      <div
        style={{
          display: "flex",
          alignItems: "center",
          justifyContent: "space-between",
        }}
      >
        <div>
          {show ? (
            <i className={icon} style={{ fontSize: "2rem" }} />
          ) : (
            <Image
              src={file?.objectURL}
              width={imgWH}
              height={imgWH}
              {...(preview && { preview: preview })}
            />
          )}
        </div>
        {option?.fileNameElement}
        {option?.removeElement}
      </div>
    ) : (
      messageTemplate(errMsg)
    );
  };

  const dragDropTemplate = (errorMsg) => {
    return (
      <div
        className="flex align-items-center flex-column cursor-pointer"
        onDrop={(e) => setSelectedFile(e.dataTransfer?.files[0])}
        onDragOver={(e) => e.preventDefault()}
        onClick={() => ref.current?.getInput().click()}
      >
        <i
          className="pi pi-cloud-upload mt-1 p-1"
          style={{
            fontSize: "5em",
            borderRadius: "50%",
            backgroundColor: "var(--surface-b)",
            color: "var(--surface-d)",
          }}
        />
        <span
          style={{ fontSize: "1.2em", color: "var(--text-color-secondary)" }}
          className="my-2"
        >
          Drag and Drop {emptyMessage}
        </span>
        {errorMsg && <p className="text-sm ml-2 text-red-500">{errorMsg}</p>}
      </div>
    );
  };

  return (
    <React.Fragment>
      <FileUpload
        name={name}
        multiple={false}
        accept={accept}
        ref={ref}
        chooseLabel={buttonLabel}
        chooseOptions={{
          style: {
            borderRadius: "6px",
            backgroundColor: "green",
            border: "1px solid lightgreen",
          },
          icon: PrimeIcons.CLOUD_UPLOAD,
        }}
        onSelect={handleSelectFile}
        uploadOptions={{ style: { display: "none" } }}
        cancelOptions={{ style: { display: "none" } }}
        style={{ width: "450px", height: "300px" }}
        progressBarTemplate={<React.Fragment />}
        itemTemplate={(e, option) => customPreviewTemplate(e, option, error)}
        previewWidth={previewWidth}
        pt={{
          content: { style: { padding: 0 } },
          buttonbar: { style: { padding: "10px" } },
          badge: { root: { style: { display: "none" } } },
          removeButton: { root: { onClick: clearSelectedFile } },
          file: { draggable: draggable },
          fileName: { className: "text-sm", draggable: draggable },
        }}
        emptyTemplate={messageTemplate(error, emptyMessage)}
      />
    </React.Fragment>
  );
};

export default FileUploadComponent;
