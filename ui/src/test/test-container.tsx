import { PrimeReactProvider } from "primereact/api";
import React, { FunctionComponent } from "react";
import { QueryClient, QueryClientProvider } from "react-query";
import { MemoryRouter } from "react-router-dom";

const TestContextProvider: FunctionComponent = ({ children }: any) => {
  const queryClient = React.useMemo(
    () => new QueryClient({ defaultOptions: { queries: { retry: false } } }),
    []
  );

  return (
    <React.Fragment>
      <MemoryRouter>
        <PrimeReactProvider>
          <QueryClientProvider client={queryClient}>
            {children}
          </QueryClientProvider>
        </PrimeReactProvider>
      </MemoryRouter>
    </React.Fragment>
  );
};

export default TestContextProvider;
