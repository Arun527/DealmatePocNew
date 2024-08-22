import { PrimeIcons } from "primereact/api";
import { ConfirmDialog } from "primereact/confirmdialog";
import React from "react";
import { confirmationDialogStyle, dialogWidth } from "../../style";

interface Props {
  open: boolean;
  handleAction: (text: string) => void;
  actionLabels?: any[];
  outSideClick?: boolean;
  width?: string;
  content: {
    header: any;
    text: string;
  };
}

const ConfirmationDialog = ({
  open,
  handleAction,
  actionLabels,
  outSideClick = true,
  width = "md",
  content,
}: Props) => {
  const [onOpen, setOnOpen] = React.useState<boolean>(open);

  React.useEffect(() => {
    setOnOpen(open);
  }, [open]);

  const handleOutSideClick = (e: React.MouseEvent<HTMLElement>) => {
    const element = e.target as HTMLElement;
    if (element.dataset?.pcSection === "mask" && outSideClick) {
      handleAction("close");
    }
  };

  return (
    <div>
      <ConfirmDialog
        visible={onOpen}
        header={content?.header}
        message={content?.text}
        onMaskClick={handleOutSideClick}
        pt={{
          closeButton: { onClick: () => handleAction("close") },
          footer: { style: confirmationDialogStyle.footer },
        }}
        style={{ borderRadius: "15px", width: dialogWidth[width] }}
        defaultFocus="accept"
        draggable={false}
        headerStyle={confirmationDialogStyle.header}
        icon={PrimeIcons.EXCLAMATION_TRIANGLE}
        acceptLabel={actionLabels?.[0] || "Yes"}
        rejectLabel={actionLabels?.[1] || "No"}
        accept={() => handleAction("yes")}
        reject={() => handleAction("no")}
      />
    </div>
  );
};

export default ConfirmationDialog;
