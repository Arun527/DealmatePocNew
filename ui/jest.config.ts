const jestConfig = {
  clearMocks: true,
  resetMocks: true,
  collectCoverage: true,
  collectCoverageFrom: ["src/**/*.{js,jsx,ts,tsx}"],
  coverageDirectory: "coverage",
  coveragePathIgnorePatterns: ["\\\\node_modules\\\\"],
  moduleFileExtensions: ["js", "jsx", "ts", "tsx", "json", "node"],
  moduleNameMapper: {
    "\\.(css|less|scss|sass)$": "identity-obj-proxy",
  },
  rootDir: ".",
  setupFilesAfterEnv: ["<rootDir>/src/setupTests.ts"],
  testEnvironment: "jsdom",
  testEnvironmentOptions: {},
  testMatch: ["**/?(*.)+(spec|test).[tj]s?(x)"],
  transform: {
    "^.+\\.(js|jsx|ts|tsx)$": "ts-jest",
  },
  transformIgnorePatterns: ["\\\\node_modules\\\\"],
};

export default jestConfig;
