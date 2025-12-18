import { zodResolver } from "@hookform/resolvers/zod"
import * as x from "zod";

export const schema = x.object({
      id:x.number(),
    title:x.string().nonempty("Title is Required").min(3,"Minimum 3 letters Required!.").max(40,"Maximum 40 letters only allowed"),
    description:x.string().nonempty("Description is required").min(5,"Minimum 5 letters Required!.").max(60,"Maximum 60 letters only allowed"),
    createdDate:x.string(),
    isCompleted:x.boolean()
})



