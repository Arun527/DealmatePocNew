const setJSONValue = (value) => {
  return JSON.stringify(value);
};

export const getJSONValue = (value) => {
  const jsonText = value;
  return jsonText?.includes("{") ? JSON.parse(value) : value;
};

export const setData = (key, value) => {
  if (key && Boolean(value)) {
    localStorage.setItem(key, setJSONValue(value));
  } else {
    localStorage.removeItem(key);
  }
};

export const getData = (key) => {
  return getJSONValue(localStorage.getItem(key));
};

export const clearData = () => {
  localStorage.clear();
};
