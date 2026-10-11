import { useState } from "react";

function OrderPage() {
  const [form, setForm] = useState({
    name: "",
    email: "",
    qty: 1,
  });

  const [errors, setErrors] = useState({});
  const [done, setDone] = useState(false);

  function validate(v) {
    const e = {};

    if (v.name.trim().length < 2) {
      e.name = "Name too short";
    }

    if (!/^[^@\s]+@[^@\s]+\.[^@\s]+$/.test(v.email)) {
      e.email = "Enter a valid email";
    }

    if (Number(v.qty) < 1) {
      e.qty = "Qty must be at least 1";
    }

    return e;
  }

  function handleChange(e) {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  }

  function handleSubmit(e) {
    e.preventDefault();

    const found = validate(form);

    setErrors(found);

    if (Object.keys(found).length === 0) {
      setDone(true);
    }
  }

  if (done) {
    return <h2>Thanks, {form.name}! Order received.</h2>;
  }

  return (
    <form onSubmit={handleSubmit}>
      <h2>Place Order</h2>

      <input
        name="name"
        placeholder="Name"
        value={form.name}
        onChange={handleChange}
      />
      <br />
      {errors.name && <p>{errors.name}</p>}

      <input
        name="email"
        placeholder="Email"
        value={form.email}
        onChange={handleChange}
      />
      <br />
      {errors.email && <p>{errors.email}</p>}

      <input
        name="qty"
        type="number"
        value={form.qty}
        onChange={handleChange}
      />
      <br />
      {errors.qty && <p>{errors.qty}</p>}

      <button type="submit">Place Order</button>
    </form>
  );
}

export default OrderPage;