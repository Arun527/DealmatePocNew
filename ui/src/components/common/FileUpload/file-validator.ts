export const fileValidator = (file: File, extns: string) => {
  const allowedExtensions = extns.split(",");
  const fileXtn = file.name?.split(".").pop();
  const isAllowedFormat = allowedExtensions.some((x) => x === `.${fileXtn}`);
  const isAllowedFileSize = file?.size <= 2000000;
  return {
    isValid: isAllowedFormat && isAllowedFileSize,
    message: !isAllowedFormat
      ? `The file extension '${fileXtn}' is forbidden. Allowed extensions are: ${extns}`
      : !isAllowedFileSize
      ? "The file size should be less than 2 MB"
      : "",
  };
};
