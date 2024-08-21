import React from "react";
import TestContextProvider from "../../../../test/test-container";
import { ToastContextProvider, useToastContext } from "./toast";
import { render } from "@testing-library/react";
import { waitForElementByText } from "../../../../test/test-util";

const toastMessage = {
  severity: "success",
  detail: "tested successfully",
};

const TestComponent: React.FunctionComponent = () => {
  const showToast = useToastContext();

  return <button onClick={() => showToast(toastMessage)}>Show Toast</button>;
};

describe("Toast-Context", () => {
  let baseElement, screen;

  beforeEach(() => {
    ({ baseElement, ...screen } = render(
      <ToastContextProvider children={<TestComponent />} />,
      { wrapper: TestContextProvider }
    ));
  });

  it("Match snapshots", () => {
    expect(baseElement).toMatchSnapshot();
  });

  it("should show toast", async () => {
    const button = await waitForElementByText("Show Toast");
    button.click();
    const message = await waitForElementByText("tested successfully");
    expect(message?.textContent.trim()).toBe("tested successfully");
  });
});
