import { Button } from "primereact/button";
import { Card } from "primereact/card";
import { FloatLabel } from "primereact/floatlabel";
import { InputText } from "primereact/inputtext";
import { Password } from "primereact/password";
import React from "react";
import { useNavigate } from "react-router-dom";
import { regexPatterns } from "../../../common/constants";
import { loginStyle } from "../../../pages/Login/style";
import ErrorMessage from "../../common/field-error-message";
import { PrimeIcons } from "primereact/api";

const Signup = () => {
  const [formData, setFormData] = React.useState({} as any);
  const [fieldErrors, setFieldErrors] = React.useState({} as any);
  // const [showPassword, setShowPassword] = React.useState<boolean>(false);
  const navigate = useNavigate();

  const handleChange = (e, isValidValue) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    const errorMessage = !isValidValue
      ? `${name} is not valid`
      : !Boolean(value)
      ? `${name} is required`
      : "";
    setFieldErrors((prev) => ({ ...prev, [name]: errorMessage }));
  };

  const handleBlur = (e) => {
    const { name, value } = e?.target;
    if (!Boolean(value)) {
      setFieldErrors((prev) => ({
        ...prev,
        [name]: `${name} is required`,
      }));
    }
    if (formData?.password && formData?.confirmPassword) {
      setFieldErrors((prev) => ({
        ...prev,
        confirmPassword:
          formData?.password !== formData?.confirmPassword
            ? "Password and Confirm Password should be not same"
            : "",
      }));
    }
  };

  const getErrorNode = (field) => (
    <ErrorMessage message={fieldErrors?.[field]} />
  );

  const handleSubmit = (e: any) => {
    e?.preventDefault();
    console.log(formData);
    navigate("/");
  };

  return (
    <div style={loginStyle.loginContainer}>
      <Card
        title="DealMate Signup"
        className="shadow-5"
        style={{ width: "400px" }}
      >
        <form noValidate onSubmit={handleSubmit}>
          <div className="p-fluid">
            <div className="p-field" style={{ marginBottom: "25px" }}>
              <FloatLabel>
                <InputText
                  name="email"
                  id="email"
                  placeholder="Enter Email"
                  keyfilter={regexPatterns.email}
                  onInput={handleChange}
                  onBlur={handleBlur}
                  value={formData?.email}
                  invalid={Boolean(fieldErrors?.email)}
                  className="p-input"
                  validateOnly={true}
                  required
                />
                <label htmlFor="email">Email Id</label>
              </FloatLabel>
              {getErrorNode("email")}
            </div>
            <div className="p-field" style={{ marginBottom: "25px" }}>
              <FloatLabel>
                <Password
                  id="password"
                  name="password"
                  placeholder="Enter Password"
                  onChange={(e) => handleChange(e, true)}
                  onBlur={handleBlur}
                  value={formData?.password}
                  invalid={Boolean(fieldErrors?.password)}
                  feedback={false}
                  pt={{ input: { className: "p-input" } }}
                  toggleMask
                  required
                />
                <label htmlFor="password">Create Password</label>
              </FloatLabel>
              {getErrorNode("password")}
            </div>
            <div className="p-field" style={{ marginBottom: "25px" }}>
              <FloatLabel>
                <Password
                  id="confirmPassword"
                  name="confirmPassword"
                  placeholder="Enter Confirm Password"
                  onChange={(e) => handleChange(e, true)}
                  onBlur={handleBlur}
                  value={formData?.confirmPassword}
                  invalid={Boolean(fieldErrors?.confirmPassword)}
                  feedback={false}
                  pt={{ input: { className: "p-input" } }}
                  toggleMask
                  required
                />
                <label htmlFor="confirmPassword">Confirm Password</label>
              </FloatLabel>
              {getErrorNode("confirmPassword")}
            </div>
            {/* <div
              className="p-field mb-4 text-right"
              style={{ marginBottom: "25px" }}
            >
              <Checkbox
                inputId="show"
                name="showPassword"
                value={showPassword}
                style={{ margin: "0px 5px" }}
                onChange={(e) => setShowPassword(!showPassword)}
                checked={showPassword}
              />
              <label htmlFor="show">Show Password</label>
            </div> */}
          </div>
          <div style={loginStyle.button}>
            <Button label="SignUp" icon={PrimeIcons.USER_PLUS} type="submit" />
          </div>
        </form>
      </Card>
    </div>
  );
};

export default Signup;
