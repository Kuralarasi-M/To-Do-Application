import axios from "axios";
import { useNavigate } from "react-router-dom";
import { config } from "zod";


const AxiosInstance= axios.create({
    baseURL:"https://localhost:7275/api/authentication",
    headers:{
        "Content-Type":"application/json",
        "Accept":"application/json"
    },
});

AxiosInstance.interceptors.request.use(

    (config) => {
        const token=localStorage.getItem("accessToken");
        if(token){
            config.headers['Authorization']=`Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        console.log("Error msg: ",error);
        return Promise.reject(error)
    }
)


AxiosInstance.interceptors.response.use(
  response => response,
  async error => {
      const originalRequest = error.config;

      if(originalRequest.url === 'Authentication/refresh'){
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
       navigate("/")
        return Promise.reject(error);
      }


    if (error.response?.status === 401 && !originalRequest._retry) {
      originalRequest._retry = true;
        try {
        const refreshToken = localStorage.getItem("refreshToken");
        if (!refreshToken) throw new Error("No refresh token");
        console.log("refresh error");
        
        const refreshResponse = await AxiosInstance.post(
          "Authentication/refresh",
          { refreshToken:refreshToken }
          
        );

        const newAccessToken = refreshResponse.data.accessToken;
        const newRefreshToken = refreshResponse.data.refreshToken;

        console.log('New token received');
        
        console.log(newAccessToken);
        

        localStorage.setItem("accessToken", newAccessToken);
         localStorage.setItem("refreshToken", newRefreshToken);



        originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
        return AxiosInstance(originalRequest); 
      } 
      catch (err) {
        console.error("Refresh token failed", err);
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        navigate("/");
        return Promise.reject(err);
      }
    }
     return Promise.reject(error);
    }
   
  
);


export default AxiosInstance;