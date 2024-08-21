import { fireEvent, render } from "@testing-library/react";
import TestContextProvider from "../../../../test/test-container";
import ConfirmationDialog from "./confirmation-dialog";

const getProps = (open = false) => ({
  open,
  handleAction: jest.fn(),
  actionLabels: ["Yes", "No"],
  outSideClick: true,
  width: "md",
  content: {
    header: "Confirmation",
    text: "Are you sure you want to proceed?",
  },
});

const renderApp = (props: any) => {
  return render(<ConfirmationDialog {...props} />, {
    wrapper: TestContextProvider,
  });
};

describe("ConfirmationDialog-Context", () => {
  let props, baseElement;

  props = getProps();
  ({ baseElement } = renderApp(props));

  afterEach(() => {
    jest.clearAllMocks();
    jest.resetAllMocks();
  });

  it("should match snapshot", () => {
    expect(baseElement).toMatchSnapshot();
  });

  it("should show the confirmation dialog and handle the 'Yes' action", async () => {
    props = getProps(true);
    const { baseElement } = renderApp(props);

    const message = baseElement.querySelectorAll('[data-pc-section="message"]');
    expect(message?.[0]?.textContent?.trim()).toBe(props.content.text);

    const buttons = baseElement.querySelectorAll(".p-dialog-footer button");
    expect(buttons[1].textContent?.trim()).toBe(props.actionLabels[0]);
    fireEvent.click(buttons[1]);
    expect(props.handleAction).toHaveBeenCalledWith("yes");
  });

  it("should close the confirmation dialog when 'No' is clicked", async () => {
    props = getProps(true);
    const { baseElement } = renderApp(props);

    const message = baseElement.querySelectorAll('[data-pc-section="message"]');
    expect(message?.[0]?.textContent?.trim()).toBe(props.content.text);

    const buttons = baseElement.querySelectorAll(".p-dialog-footer button");
    expect(buttons[0].textContent?.trim()).toBe(props.actionLabels[1]);
    fireEvent.click(buttons[0]);
    expect(props.handleAction).toHaveBeenCalledWith("no");
  });

  it("should close the dialog when close button is clicked", async () => {
    props = getProps(true);
    const { baseElement } = renderApp(props);
    const closeIcon = baseElement.querySelectorAll(
      '[data-pc-section="closebuttonicon"]'
    );
    fireEvent.click(closeIcon[0]);
    expect(props.handleAction).toHaveBeenCalledWith("close");
  });
});
