import axios from 'axios';

export async function getCategories (){
    try {
    const response =await
    axios.get (`https://localhost:7297/api/categories?ShowOnMenu=true&PageSize=10&PageNumber=1`);
        
    const data = response.data;
        if (data.isSuccess) 
          return data.result;
        else 
            return null;
    } catch (error) {
        console.log('Error', error.message);
        return null;
    }
}

export async function getRandomPosts(number) {
    try {
        const response = await 
        axios.get (`https://localhost:7297/api/categories?ShowOnMenu=true&PageSize=10&PageNumber=1`);
        
        const data = response.data;
            if (data.isSuccess) 
              return data.result;

            else 
                return null;
        } catch (error) {
            console.log('Error', error.message);
            return null;
        }
    }

export async function getFeaturedPosts(number) {
    try {
        const response = await 
        axios.get (`https://localhost:7297/api/posts/featured/${number}`);
            
        const data = response.data;
            if (data.isSuccess) 
                return data.result;
    
            else 
                return null;
        } catch (error) {
            console.log('Error', error.message);
            return null;
        }
    }

    