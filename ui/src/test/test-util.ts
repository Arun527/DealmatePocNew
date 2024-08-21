import { waitFor, screen } from "@testing-library/react";

export const waitForElementByTestId = async (testId) => {
  let element;
  await waitFor(() => {
    element = screen.getByTestId(testId);
    expect(element).toBeInTheDocument();
  });
  return element;
};

export const waitForElementByText = async (text) => {
  let element;
  await waitFor(() => {
    element = screen.getByText(text);
    expect(element).toBeInTheDocument();
  });
  return element;
};

export const waitForElementByTagName = async (tagName) => {
  let element;
  await waitFor(() => {
    element = screen.getByText(
      (content, element) => element?.tagName.toLowerCase() === tagName
    );
    expect(element).toBeInTheDocument();
  });
  return element;
};

export const waitForElementById = async (id) => {
  let element;
  await waitFor(() => {
    element = screen.getByText(
      (content, element) => element?.id.toLowerCase() === id
    );
    expect(element).toBeInTheDocument();
  });
  return element;
};

export const toBeChecked = (label) => {
  expect(screen.getByLabelText(label)).toBeChecked();
};

export const toBeUnChecked = (label) => {
  expect(screen.getByLabelText(label)).not.toBeChecked();
};

export const toBeDisabled = (button) => {
  expect(button).toBeDisabled();
};

export const toBeEnabled = (button) => {
  expect(button).not.toBeDisabled();
};

export const waittoBeEnabled = async (button) => {
  await waitFor(() => {
    expect(button).not.toBeDisabled();
  });
};
