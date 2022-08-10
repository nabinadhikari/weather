import React from "react";
import IndexPage from "../pages/index";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import "@testing-library/jest-dom";

test("renders without crash", async () => {
  render(<IndexPage />);
});

test("renders button. button is disabled.", async () => {
  render(<IndexPage />);
  expect(screen.getByRole("button")).toHaveTextContent("Search");
  expect(screen.getByRole("button")).toBeDisabled();
});

test("city input", async () => {
  render(<IndexPage />);
  const cityInput = screen.getByPlaceholderText("City");
  const submitBtn = screen.getByRole("button");
  expect(cityInput).toBeInTheDocument();
  expect(submitBtn).toBeInTheDocument();

  expect(submitBtn).toHaveTextContent("Search");
  expect(cityInput).toHaveTextContent("");

  await userEvent.type(cityInput, "melbourne");
  expect(submitBtn).not.toBeDisabled();
});
