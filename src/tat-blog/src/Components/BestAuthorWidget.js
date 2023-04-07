// import { useEffect, useState } from 'react';
// import { getBestAuthor } from '../../Services/AuthorRepository';
// import { Link } from 'react-router-dom';

// const BestAuthor = () => {
//   const [authorsList, setAuthorsList] = useState([]);

//   useEffect(() => {
//     getBestAuthor().then((data) => {
//       if (data) setAuthorsList(data);
//       else setAuthorsList([]);
//     });
//   }, []);

//   return (
//     <div className='mb-4'>
//         <h3 className='text-success mb-2'>
//             Các chủ đề
//         </h3>
//         {categoryList.length > 0 &&
//             <ListGroup>
//                 {categoryList.map((item, index) => {
//                     return (
//                         <ListGroup.Item key={index}>
//                             <Link to={'/blog/category?slug=${item.urlSlug}'}
//                             title={item.description}
//                             key={index}>
//                                 {item.name}
//                                 <span>&nbsp;({item.postCount})</span>
//                             </Link>
//                         </ListGroup.Item>
//                     );
//                 })}
//             </ListGroup>
//         }
//     </div>
// );
// };

// export default BestAuthor;