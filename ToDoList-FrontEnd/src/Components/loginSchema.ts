
import * as z from "zod";

const loginSchema = z.object({

  name: z.string().optional(),
  email: z.string().nonempty("Email must required.").email("Invalid email"),
  password:z.string().nonempty("Password must required.")
  .regex(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/, {
      message:
        "Password must include atleast one captital,small,number, and special character and minimum length of 8.",
    })
}).refine(
  (data) => data.name || data.email, 
  {
    message: "Name is required for Sign Up",
    path: ["name"],
  }
);

export default loginSchema;