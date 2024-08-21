const { ESLint } = require("eslint");

module.exports = new ESLint({
  overrideConfig: {
    parserOptions: {
      ecmaVersion: "latest",
      sourceType: "module",
      ecmaFeatures: {
        jsx: true,
      },
    },
    env: {
      browser: true,
      es2021: true,
      jest: true,
    },
    extends: [
      "eslint:recommended",
      "plugin:react/recommended",
      "plugin:@typescript-eslint/recommended",
    ],
    settings: {
      react: {
        version: "detect",
      },
    },
    plugins: ["react", "@typescript-eslint"],
    rules: {
      "unicorn/filename-case": "off",
      "no-warning-comments": "off",
      "no-console": "off",
    },
  },
});
